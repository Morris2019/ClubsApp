using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Firebase.Auth;
using Java.Util;
using Xamarin.Essentials;

namespace GTUCClubsApp
{
    public class AnnoucementAdapter : RecyclerView.Adapter
    {
        List<AnnoucementModel> announcement = new List<AnnoucementModel>();
        public string AnnouncementID { get; set; }
        public event EventHandler<AnnoucementAdapterClickArgs> ItemClick;
        public event EventHandler<AnnoucementAdapterClickArgs> ItemlongClick;
        public string AnnouncementImage { get; set; }

        public AnnoucementAdapter(List<AnnoucementModel> announcement)
        {
            this.announcement = announcement;
        }

        public override int ItemCount => announcement.Count;

        void OnClick(AnnoucementAdapterClickArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(AnnoucementAdapterClickArgs args) => ItemlongClick?.Invoke(this, args);

        public class AnnoucementViewHolder : RecyclerView.ViewHolder
        {
            public ImageView AnnounceImage;
            public TextView AnnounceMessage, AnnouncementVenue;
            public FloatingActionButton AddComment;

            public AnnoucementViewHolder(View itemView, Action<AnnoucementAdapterClickArgs> ItemClick,
                Action<AnnoucementAdapterClickArgs> ItemlongClick) : base((itemView))
            {
                AnnounceImage = itemView.FindViewById<ImageView>(Resource.Id.MemberAnnImage);
                AnnounceMessage = itemView.FindViewById<TextView>(Resource.Id.MemberAnnouncement);
                AnnouncementVenue = itemView.FindViewById<TextView>(Resource.Id.MemberVenues);
                AddComment = itemView.FindViewById<FloatingActionButton>(Resource.Id.UserSendFab);
                //AddComment.Click += delegate { PostComments(); };


                itemView.Click += (sender, e) => ItemClick(new AnnoucementAdapterClickArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => ItemlongClick(new AnnoucementAdapterClickArgs { View = itemView, Position = AdapterPosition });
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as AnnoucementViewHolder;
            //viewholder.AnnouncementID = announcement[position].AnnoucementId;
            viewholder.AnnounceMessage.Text = announcement[position].AnnounceCaption;
            viewholder.AnnouncementVenue.Text = announcement[position].Venue;
            AnnouncementImage  = announcement[position].AnnounceImage;

            if (AnnouncementImage != null)
            {
                ImageService.Instance.LoadUrl(AnnouncementImage).Retry(5, 200).Into(viewholder.AnnounceImage);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
               // Toast.MakeText(activity, "Something went wrong, Please Try Again", ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AnnouncementCardView, parent, false);

            var viewsholders = new AnnoucementViewHolder(view, OnClick, OnLongClick);

            return viewsholders;
        } 
    }
    public class AnnoucementAdapterClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
