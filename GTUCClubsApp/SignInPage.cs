
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;

namespace GTUCClubsApp
{
    [Activity(Label = "SignInPage", Theme = "@style/AppTheme.NoActionBar")]
    public class SignInPage : AppCompatActivity, IOnCompleteListener
    {
        TextView PasswordReset, UserRegister;
        Button UserSignIn;
        EditText Memberemail, Memberpassword;
        RelativeLayout PageSnackbar;
        ProgressBar PageProgress;

        FirebaseAuth memberauth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignInPage);
            FirebaseApp.InitializeApp(this);
            memberauth = FirebaseAuth.Instance;
            // Create your application here
            PasswordReset = FindViewById<TextView>(Resource.Id.clubsforgotText);
            PasswordReset.Click += MemberpasswordReset;
            UserRegister = FindViewById<TextView>(Resource.Id.clubsSignUp);
            UserRegister.Click += MemberRegister;
            UserSignIn = FindViewById<Button>(Resource.Id.clubssigninbut);
            UserSignIn.Click += MemberSignIn;

            Memberemail = FindViewById<EditText>(Resource.Id.clubsuserEmail);
            Memberpassword = FindViewById<EditText>(Resource.Id.clubsuserpassword);

            PageSnackbar = FindViewById<RelativeLayout>(Resource.Id.ClubsRelativePage);
            PageProgress = FindViewById<ProgressBar>(Resource.Id.ClubProgress);
        }

        private void MemberpasswordReset(object sender, EventArgs e)
        {
            PageProgress.Visibility = ViewStates.Visible;
            StartActivity(new Android.Content.Intent(this, typeof(PasswordReset)));
            Finish();
            PageProgress.Visibility = ViewStates.Gone;
        }

        private void MemberRegister(object sender, EventArgs e)
        {
            PageProgress.Visibility = ViewStates.Visible;
            StartActivity(new Android.Content.Intent(this, typeof(SignUpPage)));
            Finish();
            PageProgress.Visibility = ViewStates.Gone;
        }

        private void MemberSignIn(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Memberemail.Text))
            {
                Snackbar.Make(PageSnackbar, "Please Enter Your E-Mail.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
            else if (string.IsNullOrWhiteSpace(Memberpassword.Text))
            {
                Snackbar.Make(PageSnackbar, "Please Enter Your Password.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
            else
            {
                MemberSignFunction(Memberemail.Text, Memberpassword.Text);
            }
        }
        public void MemberSignFunction(string MemberUsername, string MemberPasscode)
        {
            PageProgress.Visibility = ViewStates.Visible;
            memberauth.SignInWithEmailAndPassword(MemberUsername, MemberPasscode)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                PageProgress.Visibility = ViewStates.Gone;

                StartActivity(new Android.Content.Intent(this, typeof(MainActivity)));
                Finish();
            }
            else
            {
                PageProgress.Visibility = ViewStates.Gone;
                Snackbar.Make(PageSnackbar, "Wrong Password.", Snackbar.LengthLong)
                  .SetAction("CLEAR", (view) => { Memberemail.Text = string.Empty; })
                  .Show();
            }
        }
        public override void OnBackPressed()
        {
            StartActivity(new Android.Content.Intent(this, typeof(SignInPage)));
            Finish();
            base.OnBackPressed();
        }
    }
}
