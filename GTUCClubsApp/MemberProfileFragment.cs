using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V7toolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Android.Support.V7.Widget;
using DialogBuilder = Android.Support.V7.App.AlertDialog.Builder;
using Firebase.Database;

namespace GTUCClubsApp
{
    public class MemberProfileFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView MemberDataRecycler, MemberProfileImage;
        List<MembersDataModel> Memberaccountadapter = new List<MembersDataModel>();
        MemberProfileListener profileDataRetrieve;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static MemberProfileFragment NewInstance()
        {
            var profileFrag = new MemberProfileFragment { Arguments = new Bundle() };
            return profileFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MemberProfilePage, container, false);

            MemberDataRecycler = view.FindViewById<RecyclerView>(Resource.Id.MemberProfileRecycle);
            MemberProfileImage = view.FindViewById<RecyclerView>(Resource.Id.MemberRecyclerPic);

            V7toolbar toolbar = view.FindViewById<V7toolbar>(Resource.Id.ProfileTollbar);

            toolbar.InflateMenu(Resource.Menu.menu_main);
            toolbar.MenuItemClick += MenuItemSelected;

            ProfilePictureRetrieve();
            return view;
        }
        private void MenuItemSelected(object sender, V7toolbar.MenuItemClickEventArgs e)
        {
            ProfileItems(e.Item.ItemId);
        }
        private void ProfileItems(int id)
        {
            Intent intent;
            switch (id)
            {
                case Resource.Id.action_settings:
                    intent = new Intent(Activity, typeof(SignInPage));
                    FirebaseAuth.Instance.SignOut();
                    StartActivity(intent);
                    break;
                //case Resource.Id.nav_edit:
                //    intent = new Intent(Activity, typeof(EditUserProfile));

                //    StartActivity(intent);
                //    break;
            }
        }
        public void MemberProfileRecycler()
        {
            MemberDataRecycler.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(MemberDataRecycler.Context));
            MemberProfileAdapter profiledataadapter = new MemberProfileAdapter(Memberaccountadapter);
            MemberDataRecycler.SetAdapter(profiledataadapter);
        }
        public void MemberProfilePicRecycler()
        {
            MemberProfileImage.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(MemberProfileImage.Context));
            MmeberProfilePicAdapter profilepicadapter = new MmeberProfilePicAdapter(Memberaccountadapter);
            MemberProfileImage.SetAdapter(profilepicadapter);

        }
        public void ProfilePictureRetrieve()
        {
            profileDataRetrieve = new MemberProfileListener();
            profileDataRetrieve.MemberProfilePic();
            profileDataRetrieve.memberAccountretrieve += RetrieveMemberPicture;

        }

        private void RetrieveMemberPicture(object sender, MemberProfileListener.MemberProfileEvent e)
        {
            Memberaccountadapter = e.memberaccountModels;
          
            MemberProfilePicRecycler();
            MemberProfileRecycler();
        }
    }
}
