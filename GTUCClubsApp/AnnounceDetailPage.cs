using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using FFImageLoading;
using Firebase.Auth;
using Java.Util;
using Xamarin.Essentials;
using DialogBuilder = Android.Support.V7.App.AlertDialog.Builder;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Java.Lang;
using Android.Support.Design.Widget;
using Android.Views;
using Firebase.Database;
using System.Linq;

namespace GTUCClubsApp
{
    [Activity(Label = "AnnounceDetailPage", Theme = "@style/AppTheme.NoActionBar")]
    public class AnnounceDetailPage : AppCompatActivity, IOnSuccessListener
    {
        AnnouncementListeners AnnounceListener;
        CardView AnnounceDetail;
        CoordinatorLayout Detailpage;
        RecyclerView AnnounceComments;
        ImageView AnnouncementImage;
        EditText MemberComment;
        TextView AnnouncementTitle, AnnouncementDiscrip, AnnouncementVenue, AnnouncementDate, AnnouncementFaculty, AnnouncementAssociation, AnnouncementDatePosted, PostAnnounce;
        public string UserImageUrl { get; set; }

        private static readonly int ButtonNotificationClick = 9999;
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";
        public string AnnounmentID { get; set; }

        FloatingActionButton NotificationButton;

        List<CommentModel> announcement = new List<CommentModel>();
        CommentListener announeceComment;
        //int count = 1;

        List<MembersDataModel> Memberaccountadapter = new List<MembersDataModel>();
        MemberProfileListener profileDataRetrieve;

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

        public string UserName { get; set; }
        public string Memberlname { get; set; }

        ISharedPreferences UserDataStore = Application.Context.GetSharedPreferences("fenriesUserData", FileCreationMode.Private);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AnnounceDetailPage);
            // Create your application here

            AnnouncementTitle = FindViewById<TextView>(Resource.Id.AnnouncementTitle);
            AnnouncementDiscrip = FindViewById<TextView>(Resource.Id.AnnouncementDiscrip);
            AnnouncementVenue = FindViewById<TextView>(Resource.Id.AnnouncementVenue);
            AnnouncementDate = FindViewById<TextView>(Resource.Id.AnnouncementDate);
            AnnouncementFaculty = FindViewById<TextView>(Resource.Id.AnnouncementFaculty);
            AnnouncementAssociation = FindViewById<TextView>(Resource.Id.AnnouncementAssociation);
            AnnouncementDatePosted = FindViewById<TextView>(Resource.Id.AnnouncementDatePosted);

            AnnounceDetail = FindViewById<CardView>(Resource.Id.AnnounceDetail);
            AnnounceDetail.LongClick += DeleteAnnonce;

            PostAnnounce = FindViewById<TextView>(Resource.Id.MemberCommentText);
            PostAnnounce.Click += PostComments;

            NotificationButton = FindViewById<FloatingActionButton>(Resource.Id.nav_notification);
            NotificationButton.Click += AnnounceNotification;

            AnnounceComments = FindViewById<RecyclerView>(Resource.Id.AnnouncementComment);
            MemberComment = FindViewById<EditText>(Resource.Id.MemberComment);
            AnnouncementImage = FindViewById<ImageView>(Resource.Id.AnnouncementImage);
            Detailpage = FindViewById<CoordinatorLayout>(Resource.Id.AnnouncePage);

            AnnouncementTitle.Text = Intent.GetStringExtra("AnnounceCaption");
            AnnouncementDiscrip.Text = Intent.GetStringExtra("EventDiscription");
            AnnouncementVenue.Text = Intent.GetStringExtra("Venue");
            AnnouncementDate.Text = Intent.GetStringExtra("Event_Date");
            AnnouncementFaculty.Text = Intent.GetStringExtra("Faculty");
            AnnouncementAssociation.Text = Intent.GetStringExtra("MemberAssociation");
            AnnouncementDatePosted.Text = Intent.GetStringExtra("DatePosted");
            UserImageUrl = Intent.GetStringExtra("AnnounceImage");

            AnnounmentID = Intent.GetStringExtra("AnnounmentID");

            Toast.MakeText(this, AnnounmentID, ToastLength.Short).Show();

            if (UserImageUrl != null)
            {
                ImageService.Instance.LoadUrl(UserImageUrl).Retry(5, 200).Into(AnnouncementImage);
            }
            ProfilePictureRetrieve();
            LoadComment();
            CreateNotificationChannel();

            string FirstName = UserDataStore.GetString("FirstName", "");
            string LastName = UserDataStore.GetString("LastName", string.Empty);
            string MemberAssociation = UserDataStore.GetString("MemberAssociation", string.Empty);
            string MemberEmail = UserDataStore.GetString("MemberEmail", string.Empty);
            string MemberProPic = UserDataStore.GetString("MemberProPic", string.Empty);
            UserName = UserDataStore.GetString("UserName", "");
        }

        private void DeleteAnnonce(object sender, View.LongClickEventArgs e)
        {
            DialogBuilder Deletedialog = new DialogBuilder(this);
            Deletedialog.SetTitle("Delete Event");
            Deletedialog.SetMessage("Contents Deleted can not be reovered. Click on Continue to delete");
            Deletedialog.SetPositiveButton("Continue", (deleteAlertDialog, args) =>
            {
                DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("ClubsAnnouncement/" + AnnounmentID);
                profileref.RemoveValue();
            });
            Deletedialog.SetNegativeButton("Cancel", (deleteAlertDialog, args) =>
            {
                Deletedialog.Dispose();
            });
            Deletedialog.Show();
        }

        private void AnnounceNotification(object sender, EventArgs e)
        {
            AnnounceCaption = Intent.GetStringExtra("AnnounceCaption");
            EventDiscription = Intent.GetStringExtra("EventDiscription");

            //var valuesForActivity = new Bundle();
            //valuesForActivity.PutInt(COUNT_KEY);

            // When the user clicks the notification, SecondActivity will start up.
            var resultIntent = new Intent(this, typeof(MainActivity));

            // Pass some values to SecondActivity:
            //resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            var stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Class.FromType(typeof(AnnounceDetailPage)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                          .SetContentTitle(AnnounceCaption) // Set the title
                          .SetSmallIcon(Resource.Drawable.baseline_add_alert_black_18dp) // This is the icon to display
                          .SetContentText(EventDiscription); // the message to display.

            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
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

            var name = Resources.GetString(Resource.String.channel_name);
            var description = GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        public void AnnouCommentLoad()
        {
            AnnounceComments.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(AnnounceComments.Context));
            CommentRecyclerVAiew announceLoader = new CommentRecyclerVAiew(announcement);
            AnnounceComments.SetAdapter(announceLoader);
        }
        public void LoadComment()
        {
            announeceComment = new CommentListener();
            announeceComment.CommentLoad();
            announeceComment.announcementevent += RetrieveAnnounceComment;
        }
        private void RetrieveAnnounceComment(object sender, CommentListener.CommentEvent e)
        {
            announcement = e.EventComment;
            AnnouCommentLoad();
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
        }
        public void PostComments(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                string UserProfile = AccountDataHelper.MemberProPic();
                string Comment = MemberComment.Text;
                string Date = DateTime.Now.ToString();
                string memberUsername = AccountDataHelper.UserName();

                HashMap userDataModel = new HashMap();
                userDataModel.Put("MemberUserId", FirebaseAuth.Instance.CurrentUser.Uid.ToString());
                userDataModel.Put("AnnouncementID", AnnounmentID);
                userDataModel.Put("Comment", Comment);
                userDataModel.Put("DateCreated", Date);
                userDataModel.Put("UserName", memberUsername);
                userDataModel.Put("MemProfilePic", UserProfile);

                if (string.IsNullOrEmpty(MemberComment.Text))
                {
                }
                else
                {
                    var UserDataBase = AccountDataHelper.GetDatabase().GetReference("AnnounceComment").Push();
                    UserDataBase.SetValue(userDataModel).AddOnSuccessListener(this);
                }
            }
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            MemberComment.Text = string.Empty;
        }
    }
}