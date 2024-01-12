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
using Xamarin.Essentials;

namespace GTUCClubsApp
{
    [Activity(Label = "PasswordReset", Theme = "@style/AppTheme.NoActionBar")]
    public class PasswordReset : AppCompatActivity, IOnCompleteListener
    {
        Button Memberemaillink;
        TextView RegistrationPage;
        EditText MemberEmailbx;
        RelativeLayout ResetPageFeed;
        FirebaseAuth memberauth;
        ProgressBar ResetProgress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PasswordReset);
            FirebaseApp.InitializeApp(this);
            memberauth = FirebaseAuth.Instance;
            // Create your application here
            
            Memberemaillink = FindViewById<Button>(Resource.Id.MemberPasscodereset);
            Memberemaillink.Click += ResetPassword;

            RegistrationPage = FindViewById<TextView>(Resource.Id.clubsPasswordSignUp);
            RegistrationPage.Click += delegate {
                StartActivity(new Android.Content.Intent(this, typeof(SignUpPage)));
                Finish();
            };
            MemberEmailbx = FindViewById<EditText>(Resource.Id.Memberemailbox);
            ResetProgress = FindViewById<ProgressBar>(Resource.Id.circularProgress);
            ResetPageFeed = FindViewById<RelativeLayout>(Resource.Id.MemberresetPage);
        }
        private void ResetPassword(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                if (string.IsNullOrEmpty(MemberEmailbx.Text))
                {
                    Snackbar.Make(ResetPageFeed, "Please Enter Your E-Mail.", Snackbar.LengthLong)
                   .SetAction("OK", (view) => { })
                   .Show();
                }
                else
                {
                    MemberPasswordReset(MemberEmailbx.Text);
                }
            }
            else
            {
                Snackbar.Make(ResetPageFeed, "Please Check Your Network and Try Again.", Snackbar.LengthLong)
               .SetAction("OK", (view) => { })
               .Show();
            }
        }
        private void MemberPasswordReset(string MemberEmail)
        {
            memberauth.SendPasswordResetEmail(MemberEmail).AddOnCompleteListener(this, this);
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
                ResetProgress.Visibility = ViewStates.Gone;
                StartActivity(new Android.Content.Intent(this, typeof(SignInPage)));
                Finish();
            }
            else
            {
                ResetProgress.Visibility = ViewStates.Gone;
                Toast.MakeText(this, "Something went wrong, Please Try Again", ToastLength.Short).Show();
            }
        }
    }
}