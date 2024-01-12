using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Iid;
using Firebase.Messaging;

namespace GTUCClubsApp
{
    [Activity(Label = "PROMainPage", Theme = "@style/AppTheme.NoActionBar")]
    public class PROMainPage : AppCompatActivity
    {
        BottomNavigationView bottomNavigation;
        TextView msgText;

        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        FirebaseAuth memberauth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PROMainPage);
            FirebaseApp.InitializeApp(this);
            memberauth = FirebaseAuth.Instance;
            // Create your application here
            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }

            Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);

            FirebaseMessaging.Instance.SubscribeToTopic("news");
            Log.Debug(TAG, "Subscribed to remote notifications");

            IsPlayServicesAvailable();

            CreateNotificationChannel();

            LoadFragment(Resource.Id.nav_home);
        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    Toast.MakeText(Application.Context, "This device is not supported" , ToastLength.Short).Show();
                    Finish();
                }
                return false;
            }
            else
            {
                Toast.MakeText(Application.Context, "Google Play Services is Available", ToastLength.Short).Show();
                return true;
            }
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;

            switch (id)
            {
                case Resource.Id.nav_home:
                    fragment = PROHomeFragment.NewInstance();
                    break;
                case Resource.Id.nav_message:
                    fragment = MembersMessageFragment.NewInstance();
                    break;
                case Resource.Id.nav_add:
                    Intent intent = new Intent(this, typeof(PROPostPagePage));
                    StartActivity(intent);
                    break;
                case Resource.Id.nav_account:
                    fragment = MemberProfileFragment.NewInstance();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }

    }
}
