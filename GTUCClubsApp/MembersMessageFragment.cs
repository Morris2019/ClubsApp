
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GTUCClubsApp
{
    public class MembersMessageFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView MembersProfileList;
        List<MembersDataModel> Memberaccountadapter = new List<MembersDataModel>();
        private MembersDataModel MemberListPust;
        MemberProfileListener profileDataRetrieve;

        public string RecieverUserId { get; private set; }
        public string MemberFirstname { get; private set; }
        public string MemberLastname { get; private set; }
        public string MemberFaculty { get; private set; }
        public string MemberImageUrl { get; private set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static MembersMessageFragment NewInstance()
        {
            var messageFragment = new MembersMessageFragment { Arguments = new Bundle() };
            return messageFragment;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MembersMessageFragment, container, false);
            MembersProfileList = view.FindViewById<RecyclerView>(Resource.Id.MemberMessageRecycler);

            MessageProfileList();
            return view;
        }
        public void MemberProfileRecycler()
        {
            MembersProfileList.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(MembersProfileList.Context));
            MemberProfileListAdapter MmebermessagProfile = new MemberProfileListAdapter(Memberaccountadapter);
            MmebermessagProfile.ItemClick += MemberChartDetails;
            MembersProfileList.SetAdapter(MmebermessagProfile);
        }

        private void MemberChartDetails(object sender, MembersListClickArgs e)
        {
            MembersDataModel MemberProfileData = Memberaccountadapter[e.Position];
            MemberListPust = MemberProfileData;
            RecieverUserId = MemberProfileData.MemberId;
            MemberFirstname = MemberProfileData.MemberFirstName;
            MemberLastname = MemberProfileData.MemberLastName;
            MemberImageUrl = MemberProfileData.MemberProfileImages;
            MemberFaculty = MemberProfileData.MemberFaculty;

            var intent = new Intent(Activity, typeof(MemberChatPage));
            intent.PutExtra("RecieverUserId", RecieverUserId);
            intent.PutExtra("MemberProfileImages", MemberImageUrl);
            intent.PutExtra("MemberFirstname", MemberFirstname);
            intent.PutExtra("MemberFaculty", MemberFaculty);
            intent.PutExtra("MemberLastname", MemberLastname);

            this.StartActivity(intent);
        }

        public void MessageProfileList()
        {
            profileDataRetrieve = new MemberProfileListener();
            profileDataRetrieve.MemeberChatList();
            profileDataRetrieve.memberAccountretrieve += RetrieveMemberPicture;

        }

        private void RetrieveMemberPicture(object sender, MemberProfileListener.MemberProfileEvent e)
        {
            Memberaccountadapter = e.memberaccountModels;
            MemberProfileRecycler();
        }
    }
}
