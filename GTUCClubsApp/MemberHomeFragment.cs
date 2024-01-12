using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DialogBuilder = Android.Support.V7.App.AlertDialog.Builder;
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
    public class MemberHomeFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView MemberRecycler;
        List<AnnoucementModel> announcementlist = new List<AnnoucementModel>();
        private AnnoucementModel AnnModel;

        AnnouncementListeners announcelisten;
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
        //public string AnnounmentID { get; set; }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static MemberHomeFragment NewInstance()
        {
            var homefrag = new MemberHomeFragment { Arguments = new Bundle() };
            return homefrag;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.PROHomeFragment, container, false);
            MemberRecycler = view.FindViewById<RecyclerView>(Resource.Id.HomeRecyclerView);

            LoadAnnouncement();

           // Toast.MakeText(Activity, AnnounmentID, ToastLength.Short).Show();
            return view;
        }
        public void LoadAnnounceAdapter()
        {
            MemberRecycler.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(MemberRecycler.Context));
            AnnoucementAdapter announceLoader = new AnnoucementAdapter(announcementlist);
            announceLoader.ItemClick += AnnouncementClick;
            //announceLoader.ItemlongClick += DeleteAnnoune;
            MemberRecycler.SetAdapter(announceLoader);
        }

        //private void DeleteAnnoune(object sender, AnnoucementAdapterClickArgs e)
        //{
        //    string key = announcementlist[e.Position].AnnoucementId;
        //    DialogBuilder Deletedialog = new DialogBuilder(Activity);
        //    Deletedialog.SetTitle("Delete Event");
        //    Deletedialog.SetMessage("Contents Deleted can not be reovered. Click on Continue to delete");
        //    Deletedialog.SetPositiveButton("Continue", (deleteAlertDialog, args) =>
        //    {
        //        AnnounceListener.DeleteEvent(key);
        //    });
        //    Deletedialog.SetNegativeButton("Cancel", (deleteAlertDialog, args) =>
        //    {
        //        Deletedialog.Dispose();
        //    });
        //    Deletedialog.Show();
        //}

        private void AnnouncementClick(object sender, AnnoucementAdapterClickArgs e)
        {

            AnnoucementModel profiledata = announcementlist[e.Position];
            string AnnounmentID = announcementlist[e.Position].AnnoucementId;
            AnnModel = profiledata;

            AnnounceCaption = profiledata.AnnounceCaption;
            EventDiscription = profiledata.EventDiscrip;
            AnnounceImage = profiledata.AnnounceImage;
            Event_Date = profiledata.Event_Date;
            Faculty = profiledata.Faculty;
            MemberAssociation = profiledata.MemberAssociation;
            Venue = profiledata.Venue;
            DatePosted = profiledata.DateCreated;
            //AnnounmentID = profiledata.AnnoucementId;

            var intent = new Intent(Activity, typeof(AnnounceDetailPage));
            intent.PutExtra("AnnounceCaption", AnnounceCaption);
            intent.PutExtra("EventDiscription", EventDiscription);
            intent.PutExtra("AnnounceImage", AnnounceImage);
            intent.PutExtra("Event_Date", Event_Date);
            intent.PutExtra("Faculty", Faculty);
            intent.PutExtra("MemberAssociation", MemberAssociation);
            intent.PutExtra("Venue", Venue);
            intent.PutExtra("DatePosted", DatePosted);
            intent.PutExtra("AnnounmentID", AnnounmentID);

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
            announcementlist = e.Announceeventmodel;
            LoadAnnounceAdapter();
        }
    }
}
