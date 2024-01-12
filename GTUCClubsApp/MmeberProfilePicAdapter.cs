using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Refractored.Controls;

namespace GTUCClubsApp
{
    public class MmeberProfilePicAdapter : RecyclerView.Adapter
    {
        List<MembersDataModel> Memberaccountadapter;
        public event EventHandler<MmeberProfileClickArgs> ItemClick;
        public string MmeberProfileUrl { get; set; }
        public Context activity;

        public MmeberProfilePicAdapter(List<MembersDataModel> Memberaccountadapter)
        {
            this.Memberaccountadapter = Memberaccountadapter;
        }

        public override int ItemCount => Memberaccountadapter.Count;

        void OnClick(MmeberProfileClickArgs args) => ItemClick?.Invoke(this, args);

        public class MmeberProfilePicViewHolder : RecyclerView.ViewHolder
        {
            public ImageView MemberProfilePic;

            public MmeberProfilePicViewHolder(View itemView, Action<MmeberProfileClickArgs> ItemClick) : base(itemView)
            {
                MemberProfilePic = itemView.FindViewById<ImageView>(Resource.Id.MemberProfilePic);
                itemView.Click += (sender, e) => ItemClick(new MmeberProfileClickArgs { View = itemView, Position = AdapterPosition });
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as MmeberProfilePicViewHolder;
            MmeberProfileUrl = Memberaccountadapter[position].MemberProfileImages;

            if (MmeberProfileUrl != null)
            {
                ImageService.Instance.LoadUrl(MmeberProfileUrl).Retry(5, 200).Into(viewholder.MemberProfilePic);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                Toast.MakeText(activity, "Something went wrong, Please Try Again" , ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MemberProfilePicCard, parent, false);

            var viewsholders = new MmeberProfilePicViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class MmeberProfileClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
