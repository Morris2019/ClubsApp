using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace GTUCClubsApp
{
    [Activity(Label = "MemberMainPage", Theme = "@style/AppTheme.NoActionBar")]
    public class MemberMainPage : AppCompatActivity
    {
        BottomNavigationView bottomNavigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MemberMainPage);
            // Create your application here
            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadFragment(Resource.Id.nav_home);
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
                    fragment = MemberHomeFragment.NewInstance();
                    break;
                case Resource.Id.nav_message:
                    fragment = MembersMessageFragment.NewInstance();
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
