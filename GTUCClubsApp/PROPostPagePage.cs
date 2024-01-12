using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Firebase.Auth;
using Firebase.Storage;
using FR.Ganfra.Materialspinner;
using Java.Util;
using Xamarin.Essentials;
using Android.Support.Design.Widget;
using Android.Gms.Tasks;
using Android.Support.V7.App;
using System.IO;
using Android.Graphics;
using Android.Provider;
using Android.Views;

namespace GTUCClubsApp
{
    [Activity(Label = "PROPostPagePage", Theme = "@style/AppTheme.NoActionBar")]
    public class PROPostPagePage : AppCompatActivity, IOnCompleteListener
    {
        public string MemFaculty { get; private set; }
        public string MemPosition { get; private set; }
        public string MemAssociation { get; private set; }
        public string UserId { get; private set; }

        RelativeLayout SignUpPageSnack;
        TextInputLayout ClubAnnounceCaption, AnnounceVenue, AnnounceDate, Eventdiscrip;
        List<string> facultyList, associationList;
        ArrayAdapter<string> facultyAdapter, associationAdapter, positionAdapter;
        MaterialSpinner MemberFaculty, MemberPosition, MemberAssociation;
        ProgressBar Poststatus;

        private FirebaseAuth auth;
        FirebaseStorage firebaseStorage;
        public StorageReference storageRef;

        public int PICK_IMAGE_REQUSET = 1000;
        private Android.Net.Uri filePath;

        ImageView UploadAnnouncement;

        private Android.Support.V7.Widget.Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PROPostPageFragment);

          
            // Create your application here
            storageRef = FirebaseStorage.Instance.GetReference("ClubsAnnouncement/" + Guid.NewGuid().ToString());
            auth = FirebaseAuth.Instance;

            ClubAnnounceCaption = FindViewById<TextInputLayout>(Resource.Id.UserCaptionText);
            AnnounceVenue = FindViewById<TextInputLayout>(Resource.Id.MemberVenue);
            AnnounceDate = FindViewById<TextInputLayout>(Resource.Id.MemberDate);
            Eventdiscrip = FindViewById<TextInputLayout>(Resource.Id.UserEventDisciption);

            Poststatus = FindViewById<ProgressBar>(Resource.Id.UserPost);

            UploadAnnouncement = FindViewById<ImageView>(Resource.Id.UserImageSlider1);
            UploadAnnouncement.Click += SelectImage;

            SignUpPageSnack = FindViewById<RelativeLayout>(Resource.Id.postitems);

            MemberFaculty = FindViewById<MaterialSpinner>(Resource.Id.MemberFaculty);
            FacultyList();
            MemberAssociation = FindViewById<MaterialSpinner>(Resource.Id.MemberAssociation);
            AssociationList();

            Android.Support.V7.Widget.Toolbar myToolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.ProfileTollbar);
            myToolbar.Title = "";
            SetSupportActionBar(myToolbar);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.PostItemIcon, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.PostAnnouncement:
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet)
                    {
                        PROAnnouncementFunc();
                    }
                    else
                    {
                        Snackbar.Make(SignUpPageSnack, "Please Check Your Network and Try Again.", Snackbar.LengthLong)
                        .SetAction("OK", (view) => { })
                        .Show();
                    }
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private async void PROAnnouncementFunc()
        {
            string UserPostImages = (string)await storageRef.GetDownloadUrlAsync();
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            UserId = user.Uid;

            if (user != null)
            {
                //    UserId = user.Uid;

                string AnnouncementImage = UserPostImages.Substring(0, UserPostImages.IndexOf("&token"));
                string CapTion = ClubAnnounceCaption.EditText.Text;
                string EDiscription = Eventdiscrip.EditText.Text;
                string MemberVenue = AnnounceVenue.EditText.Text;
                string AnnDate = AnnounceDate.EditText.Text;
                string Date = DateTime.Now.ToString();

                HashMap userDataModel = new HashMap();
                userDataModel.Put("MemberUserId", FirebaseAuth.Instance.CurrentUser.Uid.ToString());
                userDataModel.Put("AnnounceCaption", CapTion);
                userDataModel.Put("EventDiscription", EDiscription);
                userDataModel.Put("AnnounceImage", AnnouncementImage);
                userDataModel.Put("MemberAssociation", MemAssociation);
                userDataModel.Put("Faculty", MemFaculty);
                userDataModel.Put("Venue", MemberVenue);
                userDataModel.Put("Event_Date", AnnDate);
                userDataModel.Put("DateCreated", Date);


                if (string.IsNullOrEmpty(ClubAnnounceCaption.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, " Please Enter Your Announcement Title", Snackbar.LengthLong)
                    .Show();
                }
                else if (string.IsNullOrEmpty(Eventdiscrip.EditText.Text))
                {
                    Snackbar.Make(SignUpPageSnack, " Please Enter Event Discription", Snackbar.LengthLong)
                    .Show();
                }
                
                else
                {
                    Poststatus.Visibility = ViewStates.Visible;
                    var UserDataBase = AccountDataHelper.GetDatabase().GetReference("ClubsAnnouncement").Push();
                    UserDataBase.SetValue(userDataModel).AddOnCompleteListener(this, this);
                }

            }
        }

        private void SelectImage(object sender, EventArgs e)
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
                UploadTask uploadTask = storageRef.PutFile(data.Data);
                var Task = uploadTask.ContinueWithTask(new MemberPostContinueProfileTask(this))
                .AddOnCompleteListener(new MemberPostCompletTask(this));
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
                    UploadAnnouncement.SetImageBitmap(bitmap);
                }
                catch (IOException ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }
        
        public void FacultyList()
        {
            facultyList = new List<string>();
            facultyList.Add("All Faculties");
            facultyList.Add("School Business");
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
            associationList.Add("All Association");
            associationList.Add("Business Student Association");
            associationList.Add("Engineering Student Association");
            associationList.Add("Student Association of Computing");

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

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Poststatus.Visibility = ViewStates.Visible;
                var intent = new Intent(this, typeof(PROMainPage));
                StartActivity(intent);
                Poststatus.Visibility = ViewStates.Gone;
            }
            else
            {
                Snackbar.Make(SignUpPageSnack, " Something went wrong", Snackbar.LengthLong)
                .Show();
            }
        }
        public override void OnBackPressed()
        {
            StartActivity(new Android.Content.Intent(this, typeof(PROMainPage)));
            Finish();
            base.OnBackPressed();
        }
    }
    internal class MemberPostCompletTask : Java.Lang.Object, IOnCompleteListener
    {
        private PROPostPagePage MemberSignUp;

        public MemberPostCompletTask(PROPostPagePage MemberSignUp)
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

    internal class MemberPostContinueProfileTask : Java.Lang.Object, IContinuation
    {
        private PROPostPagePage MemberSignUp;

        public MemberPostContinueProfileTask(PROPostPagePage MemberSignUp)
        {
            this.MemberSignUp = MemberSignUp;
        }

        public Java.Lang.Object Then(Task task)
        {
            if (!task.IsSuccessful)
            {
                Toast.MakeText(MemberSignUp, "Upload Failed", ToastLength.Short).Show();
            }
            return MemberSignUp.storageRef.GetDownloadUrl();
        }
    }
}
