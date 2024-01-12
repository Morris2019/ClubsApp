using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V7toolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GTUCClubsApp
{
    public class AnnouncementSearch : Android.Support.V4.App.Fragment
    {
        RecyclerView searchrecycler;
        EditText SearchBox;
        SearchAdapter adapter;
        AnnouncementListeners announcelisten;
        List<AnnoucementModel> searchList = new List<AnnoucementModel>();
        private AnnoucementModel AnnModel;

        public string UserId { get; set; }
        public string AnnounceCaption { get; private set; }
        public string MemberUserId { get; private set; }
        public string EventDiscription { get; private set; }
        public string AnnounceImage { get; set; }
        public string Event_Date { get; private set; }
        public string Faculty { get; private set; }
        public string MemberAssociation { get; private set; }
        public string Venue { get; private set; }
        public string DatePosted { get; private set; }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static AnnouncementSearch NewInstance()
        {
            var homefrag = new AnnouncementSearch { Arguments = new Bundle() };
            return homefrag;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AnnouncementSearch, container, false);

            searchrecycler = view.FindViewById<RecyclerView>(Resource.Id.searchrecycler);
            SearchBox = view.FindViewById<EditText>(Resource.Id.SearchText);
            SearchBox.TextChanged += SearchUserData;
            V7toolbar toolbar = view.FindViewById<V7toolbar>(Resource.Id.searchtoolbar);
            LoadAnnouncement();


            return view;
        }
        private void SearchUserData(object sender, TextChangedEventArgs e)
        {

            List<AnnoucementModel> SearchResult = (from searchUser in searchList
                                                   where searchUser.AnnounceCaption.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.AnnounceImage.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.Event_Date.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.Faculty.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.MemberAssociation.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.Venue.ToLower().Contains(SearchBox.Text.ToLower()) ||
                                                   searchUser.EventDiscrip.ToLower().Contains(SearchBox.Text.ToLower())
                                                   select searchUser).ToList();

            adapter = new SearchAdapter(SearchResult);
            searchrecycler.SetAdapter(adapter);
        }
        private void messageRecycler()
        {
            searchrecycler.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(searchrecycler.Context));
            SearchAdapter seaechadapter = new SearchAdapter(searchList);
            seaechadapter.ItemClick += AnnouncementClick;
            searchrecycler.SetAdapter(seaechadapter);
        }

        private void AnnouncementClick(object sender, SearchAdapterEventArgs e)
        {

            AnnoucementModel profiledata = searchList[e.Position];

            AnnModel = profiledata;

            AnnounceCaption = profiledata.AnnounceCaption;
            EventDiscription = profiledata.EventDiscrip;
            AnnounceImage = profiledata.AnnounceImage;
            Event_Date = profiledata.Event_Date;
            Faculty = profiledata.Faculty;
            MemberAssociation = profiledata.MemberAssociation;
            Venue = profiledata.Venue;
            DatePosted = profiledata.DateCreated;


            var intent = new Intent(Activity, typeof(AnnounceDetailPage));
            intent.PutExtra("AnnounceCaption", AnnounceCaption);
            intent.PutExtra("EventDiscription", EventDiscription);
            intent.PutExtra("AnnounceImage", AnnounceImage);
            intent.PutExtra("Event_Date", Event_Date);
            intent.PutExtra("Faculty", Faculty);
            intent.PutExtra("MemberAssociation", MemberAssociation);
            intent.PutExtra("Venue", Venue);
            intent.PutExtra("DatePosted", DatePosted);

            this.StartActivity(intent);
        }

        public void LoadAnnouncement()
        {
            announcelisten = new AnnouncementListeners();
            announcelisten.AnnouncementLoad();
            announcelisten.announcementevent += RetrieveMemberPicture;

        }

        private void RetrieveMemberPicture(object sender, AnnouncementListeners.AnnouncementEvent e)
        {
            searchList = e.Announceeventmodel;
            messageRecycler();
        }
    }
}
