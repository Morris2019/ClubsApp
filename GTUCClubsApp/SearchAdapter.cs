using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Refractored.Controls;

namespace GTUCClubsApp
{
    public class SearchAdapter : RecyclerView.Adapter
    {

        public event EventHandler<SearchAdapterEventArgs> ItemClick;
        List<AnnoucementModel> announcement = new List<AnnoucementModel>();
        public string GetAnnounceImage { get; set; }

        public SearchAdapter(List<AnnoucementModel> announcement)
        {
            this.announcement = announcement;
        }

        public override int ItemCount => announcement.Count;

        void OnClick(SearchAdapterEventArgs args) => ItemClick?.Invoke(this, args);

        public class SearchAdapterViewHolder : RecyclerView.ViewHolder
        {
            public CircleImageView AnnouncementImage;
            public TextView AnnounceCaption, AnnounceVenue;
            public SearchAdapterViewHolder(View itemView, Action<SearchAdapterEventArgs> ItemClick) : base(itemView)
            {
                AnnouncementImage = itemView.FindViewById<CircleImageView>(Resource.Id.announceImage);
                AnnounceCaption = itemView.FindViewById<TextView>(Resource.Id.AnnounceCapt);
                AnnounceVenue = itemView.FindViewById<TextView>(Resource.Id.AnnounceVeneu);
                itemView.Click += (sender, e) => ItemClick(new SearchAdapterEventArgs { View = itemView, Position = AdapterPosition });

            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as SearchAdapterViewHolder;

            viewholder.AnnounceCaption.Text = announcement[position].AnnounceCaption;
            viewholder.AnnounceVenue.Text = announcement[position].Venue;
            GetAnnounceImage = announcement[position].AnnounceImage;

            if (GetAnnounceImage != null)
            {
                ImageService.Instance.LoadUrl(GetAnnounceImage).Retry(5, 200).Into(viewholder.AnnouncementImage);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                // Toast.MakeText(activity, "Something went wrong, Please Try Again", ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SearchCardView, parent, false);

            var viewsholders = new SearchAdapterViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class SearchAdapterEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
