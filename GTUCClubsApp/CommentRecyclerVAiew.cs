using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Refractored.Controls;

namespace GTUCClubsApp
{
    public class CommentRecyclerVAiew : RecyclerView.Adapter
    {
        List<CommentModel> CommentList = new List<CommentModel>();
        public event EventHandler<CommentAdapterClickArgs> ItemClick;
        public string CommentImage { get; set; }

        public CommentRecyclerVAiew(List<CommentModel> CommentList)
        {
            this.CommentList = CommentList;
        }

        public override int ItemCount => CommentList.Count;

        void OnClick(CommentAdapterClickArgs args) => ItemClick?.Invoke(this, args);

        public class CommentRecyclerVAiewViewHolder : RecyclerView.ViewHolder
        {
            public TextView MemberUserName, commmentTime, Comment;
            public CircleImageView MemebrProfile;

            public CommentRecyclerVAiewViewHolder(View itemView, Action<CommentAdapterClickArgs> ItemClick) : base(itemView)
            {
                MemberUserName = itemView.FindViewById<TextView>(Resource.Id.CommentLanme);
                commmentTime = itemView.FindViewById<TextView>(Resource.Id.CommentTime);
                Comment = itemView.FindViewById<TextView>(Resource.Id.MemberComments);
                MemebrProfile = itemView.FindViewById<CircleImageView>(Resource.Id.CommentUser);

                itemView.Click += (sender, e) => ItemClick(new CommentAdapterClickArgs { View = itemView, Position = AdapterPosition });

            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as CommentRecyclerVAiewViewHolder;

            //viewholder.AnnouncementID = announcement[position].AnnoucementId;
            viewholder.MemberUserName.Text = CommentList[position].MemmberUsername;
            viewholder.commmentTime.Text = CommentList[position].DatePosted;
            viewholder.Comment.Text = CommentList[position].announceComment;
            CommentImage = CommentList[position].MemberProfile;

            if (CommentImage != null)
            {
                ImageService.Instance.LoadUrl(CommentImage).Retry(5, 200).Into(viewholder.MemebrProfile);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                // Toast.MakeText(activity, "Something went wrong, Please Try Again", ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Comments, parent, false);

            var viewsholders = new CommentRecyclerVAiewViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class CommentAdapterClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}