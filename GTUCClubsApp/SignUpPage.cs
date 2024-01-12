using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Storage;
using FR.Ganfra.Materialspinner;
using Java.Util;
using Refractored.Controls;
using Xamarin.Essentials;

namespace GTUCClubsApp
{
    [Activity(Label = "SignUpPage", Theme = "@style/AppTheme.NoActionBar")]
    public class SignUpPage : AppCompatActivity, IOnCompleteListener, IOnSuccessListener
    {
        RelativeLayout SignUpPageSnack;
        CircleImageView MemberProfilePic;
        Button CreateClubMember;
        MaterialSpinner MemberFaculty, MemberPosition, MemberAssociation;
        TextInputLayout Memberfirstname, Memberlastname, MemberuserName, Membercontact, MemberEmail, Memberpasscode;
        ProgressBar MemberRegProgress;

        public string MemFaculty { get; private set; }
        public string MemPosition { get; private set; }
        public string MemAssociation { get; private set; }
        public string UserId { get; private set; }
        public string UserEmail { get; private set; }

        List<string> facultyList, associationList, positionList;
        ArrayAdapter<string> facultyAdapter, associationAdapter, positionAdapter;

        private FirebaseAuth auth;
        FirebaseStorage firebaseStorage;
        public StorageReference storage;

        public int PICK_IMAGE_REQUSET = 1000;
        private Android.Net.Uri filePath;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUpPage);
            FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.Instance;
            firebaseStorage = FirebaseStorage.Instance;
            storage = firebaseStorage.Reference.Child("ProfileImages/" + Guid.NewGuid().ToString());
            // Create your application here

            Memberfirstname = FindViewById<TextInputLayout>(Resource.Id.clubmefirstname);
            Memberlastname = FindViewById<TextInputLayout>(Resource.Id.Memberlast_name);
            MemberuserName = FindViewById<TextInputLayout>(Resource.Id.ClubmemUserName);
            Membercontact = FindViewById<TextInputLayout>(Resource.Id.clubmemphone);
            MemberEmail = FindViewById<TextInputLayout>(Resource.Id.MemberEmail);
            Memberpasscode = FindViewById<TextInputLayout>(Resource.Id.MemberPasscode);

            MemberFaculty = FindViewById<MaterialSpinner>(Resource.Id.MemberFaculty);
            FacultyList();
            MemberAssociation = FindViewById<MaterialSpinner>(Resource.Id.MemberAssociation);
            AssociationList();
            MemberPosition = FindViewById<MaterialSpinner>(Resource.Id.MemberPosition);
            PositionList();

            SignUpPageSnack = FindViewById<RelativeLayout>(Resource.Id.ClubSignUpSnack);
            MemberRegProgress = FindViewById<ProgressBar>(Resource.Id.MemberProgress);

            CreateClubMember = FindViewById<Button>(Resource.Id.MemberSignUp);
            CreateClubMember.Click += CreateMemberFunc;
            MemberProfilePic = FindViewById<CircleImageView>(Resource.Id.UserProfile);
            MemberProfilePic.Click += SelectMemberProPic;
        }

        private void SelectMemberProPic(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PICK_IMAGE_REQUSET);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PICK_IMAGE_REQUSET && (resultCode == Result.Ok) && (data != null))
            {
                UploadTask uploadTask = storage.PutFile(data.Data);
                var Task = uploadTask.ContinueWithTask(new MyAdvertContinueProfileTask(this))
                .AddOnCompleteListener(new MyAdvertCompleteProfileTask(this));
            }
            if (requestCode == PICK_IMAGE_REQUSET &&
                resultCode == Result.Ok &&
                data != null &&
                data.Data != null)
            {
                filePath = data.Data;
                try
                {
                    Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                    MemberProfilePic.SetImageBitmap(bitmap);
                }
                catch (IOException ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }

        private void CreateMemberFunc(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MemberEmail.EditText.Text))
            {
                Snackbar.Make(SignUpPageSnack, "Please Enter Your E-Mail.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
            else if (string.IsNullOrWhiteSpace(Memberpasscode.EditText.Text))
            {
                Snackbar.Make(SignUpPageSnack, "Please Enter Your Password.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
            else
            {
                CreateMember(MemberEmail.EditText.Text, Memberpasscode.EditText.Text);
            }
        }
        private void CreateMember(string MemberEmail, string MemberPasscode)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                auth.CreateUserWithEmailAndPassword(MemberEmail, MemberPasscode)
                .AddOnSuccessListener(this);
            }

            else
            {
                Snackbar.Make(SignUpPageSnack, "Please Check Your Network and Try Again.", Snackbar.LengthLong)
                .SetAction("OK", (view) => { })
                .Show();
            }
        }

        public async void CreateMemberAccount()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                string UserProfile = (string)await storage.GetDownloadUrlAsync();

                string ProfileImages = UserProfile;
                string firstName = Memberfirstname.EditText.Text;
                string lastName = Memberlastname.EditText.Text;
                string User_Name = MemberuserName.EditText.Text;
                string MemberContact = Membercontact.EditText.Text;
                string Member_Email = MemberEmail.EditText.Text;
                string Date = DateTime.Now.ToString();

                HashMap userDataModel = new HashMap();
                userDataModel.Put("MemberUserId", UserId);
                userDataModel.Put("MemberProPic", ProfileImages);
                userDataModel.Put("FirstName", firstName);
                userDataModel.Put("LastName", lastName);
                userDataModel.Put("UserName", User_Name);
                userDataModel.Put("UserContact", MemberContact);
                userDataModel.Put("MemberEmail", UserEmail);
                userDataModel.Put("MemberAssociation", MemAssociation);
                userDataModel.Put("Faculty", MemFaculty);
                userDataModel.Put("MemberPosition", MemPosition);
                userDataModel.Put("DateCreated", Date);

                if (string.IsNullOrEmpty(Memberfirstname.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, " Please Enter Your First Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(Memberlastname.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, "Please Enter Your Last Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(MemberuserName.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, "Please Enter Your User Name", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(Membercontact.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, "Please Enter Your Phone Number", Snackbar.LengthLong)
                   .Show();
                }
                else if (string.IsNullOrEmpty(MemberEmail.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, "Please Enter Nationality", Snackbar.LengthLong)
                   .Show();
                }

                else
                {
                    var UserDataBase = AccountDataHelper.GetDatabase().GetReference("Users").Child(UserId);
                    UserDataBase.SetValue(userDataModel).AddOnCompleteListener(this, this);
                }
            }
            else
            {
                Snackbar.Make(SignUpPageSnack, "Please Check Your Network and Try Again.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }

        }
        public void FacultyList()
        {
            facultyList = new List<string>();
            facultyList.Add("School of Business");
            facultyList.Add("Faculty of Engineering");
            facultyList.Add("Faculty of Informatics");

            facultyAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, facultyList);
            facultyAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            MemberFaculty.Adapter = facultyAdapter;
            MemberFaculty.ItemSelected += FacultySelect;
        }
        public void FacultySelect(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
               MemFaculty = facultyList[e.Position];
            }
        }
        public void AssociationList()
        {
            associationList = new List<string>();
            associationList.Add("Business Student Association");
            associationList.Add("Engineering Student Association");
            associationList.Add("Association of Computing Students");
            associationList.Add("Internatioonal Students Association");
            associationList.Add("Students Representative Council");

            associationAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, associationList);
            associationAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            MemberAssociation.Adapter = associationAdapter;
            MemberAssociation.ItemSelected += AssociatiionSelect;
        }
        public void AssociatiionSelect(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
               MemAssociation = associationList[e.Position];
            }
        }
        public void PositionList()
        {
            positionList = new List<string>();
            positionList.Add("Member");
            positionList.Add("Public Relation Officer");
            positionList.Add("Treasurer");
            positionList.Add("Secretary");
            positionList.Add("President");
            positionList.Add("Vice-President");

            positionAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, positionList);
            positionAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            MemberPosition.Adapter = positionAdapter;
            MemberPosition.ItemSelected += PositionSelect;
        }
        public void PositionSelect(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
               MemPosition = positionList[e.Position];
            }
        }
        public override void OnBackPressed()
        {
            StartActivity(new Android.Content.Intent(this, typeof(SignInPage)));
            Finish();
            base.OnBackPressed();
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            UserId = user.Uid;
            UserEmail = user.Email;
            MemberRegProgress.Visibility = ViewStates.Visible;

            CreateMemberAccount();

            CreateClubMember.PerformClick();
            MemberRegProgress.Visibility = ViewStates.Invisible;

        }
    }
    internal class MyAdvertCompleteProfileTask : Java.Lang.Object, IOnCompleteListener
    {
        private SignUpPage MemberSignUp;

        public MyAdvertCompleteProfileTask(SignUpPage MemberSignUp)
        {
            this.MemberSignUp = MemberSignUp;
        }

        public string UserProfileImage { get; private set; }

        public void OnComplete(Task task)
        {
            var uri = task.Result as Android.Net.Uri;
            string Downloadurl = uri.ToString();
            Downloadurl = Downloadurl.Substring(0, Downloadurl.IndexOf("&token"));
            UserProfileImage = Downloadurl;
            //Picasso.With().Load(Downloadurl).Into(signUpPage.profileImage);
        }
    }

    internal class MyAdvertContinueProfileTask : Java.Lang.Object, IContinuation
    {
        private SignUpPage MemberSignUp;

        public MyAdvertContinueProfileTask(SignUpPage MemberSignUp)
        {
            this.MemberSignUp = MemberSignUp;
        }

        public Java.Lang.Object Then(Task task)
        {
            if (!task.IsSuccessful)
            {
                Toast.MakeText(MemberSignUp, "Upload Failed", ToastLength.Short).Show();
            }
            return MemberSignUp.storage.GetDownloadUrl();
        }
    }
}