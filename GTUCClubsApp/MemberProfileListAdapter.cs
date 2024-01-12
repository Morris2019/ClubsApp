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
    public class MemberProfileListAdapter : RecyclerView.Adapter
    {
        List<MembersDataModel> Memberaccountadapter;
        public event EventHandler<MembersListClickArgs> ItemClick;
        public string MmeberProfileUrl { get; set; }
        public Context activity;

        public MemberProfileListAdapter(List<MembersDataModel> Memberaccountadapter)
        {
            this.Memberaccountadapter = Memberaccountadapter;
        }

        public override int ItemCount => Memberaccountadapter.Count;

        void OnClick(MembersListClickArgs args) => ItemClick?.Invoke(this, args);

        public class MemberProfileListViewHolder : RecyclerView.ViewHolder
        {
            public CircleImageView MemberprofilePicture;
            public TextView MemberUserName, MemberLastname, MemberFaculty;

            public MemberProfileListViewHolder(View itemView,Action<MembersListClickArgs> ItemClick) : base(itemView)
            {
                MemberprofilePicture = itemView.FindViewById<CircleImageView>(Resource.Id.MemberProfilePicture);
                MemberUserName = itemView.FindViewById<TextView>(Resource.Id.MemberProfileName);
                MemberLastname = itemView.FindViewById<TextView>(Resource.Id.MemberProfilelastName);
                MemberFaculty = itemView.FindViewById<TextView>(Resource.Id.MemberFaculty);
                itemView.Click += (sender, e) => ItemClick(new MembersListClickArgs { View = itemView, Position = AdapterPosition });
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as MemberProfileListViewHolder;

            viewholder.MemberUserName.Text = Memberaccountadapter[position].MemberFirstName;
            viewholder.MemberLastname.Text = Memberaccountadapter[position].MemberLastName;
            viewholder.MemberFaculty.Text = Memberaccountadapter[position].MemberFaculty;
            MmeberProfileUrl = Memberaccountadapter[position].MemberProfileImages;

            if (MmeberProfileUrl != null)
            {
                ImageService.Instance.LoadUrl(MmeberProfileUrl).Retry(5, 200).Into(viewholder.MemberprofilePicture);
                // Picasso.With(activity).Load(url).Into(UserPostImage);
            }
            else
            {
                Toast.MakeText(activity, "Something went wrong, Please Try Again", ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MemberProfileListCard, parent, false);

            var viewsholders = new MemberProfileListViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class MembersListClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set;}
    }
}
