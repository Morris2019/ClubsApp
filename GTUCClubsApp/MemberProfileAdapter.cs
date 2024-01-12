using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GTUCClubsApp
{
    public class MemberProfileAdapter : RecyclerView.Adapter
    {
        List<MembersDataModel> Memberaccountadapter;
        public event EventHandler<MemberItemClickArgs> ItemClick;

        public MemberProfileAdapter(List<MembersDataModel> Memberaccountadapter)
        {
            this.Memberaccountadapter = Memberaccountadapter;
        }

        public override int ItemCount => Memberaccountadapter.Count;

        void OnClick(MemberItemClickArgs args) => ItemClick?.Invoke(this, args);

        public class MemberProfileViewHolder : RecyclerView.ViewHolder
        {
            public TextView Memberfirstname, MemberLastname, MemberUsername, MmeberEmail, MemberContact, MemberAssociation, MemberFaculty, MemberPosition;

            public MemberProfileViewHolder(View itemView, Action<MemberItemClickArgs> ItemClick) : base(itemView)
            {
                Memberfirstname = itemView.FindViewById<TextView>(Resource.Id.UserFirstName);
                MemberLastname = itemView.FindViewById<TextView>(Resource.Id.UserLastName);
                MemberUsername = itemView.FindViewById<TextView>(Resource.Id.Memberprousername);
                MmeberEmail = itemView.FindViewById<TextView>(Resource.Id.Memberproemail);
                MemberContact = itemView.FindViewById<TextView>(Resource.Id.MemeberProContact);
                MemberAssociation = itemView.FindViewById<TextView>(Resource.Id.FacultyAssociation);
                MemberFaculty = itemView.FindViewById<TextView>(Resource.Id.FacultyMember);
                MemberPosition = itemView.FindViewById<TextView>(Resource.Id.AssociationPosition);

                itemView.Click += (sender, e) => ItemClick(new MemberItemClickArgs { View = itemView, Position = AdapterPosition });
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewholder = holder as MemberProfileViewHolder;

            viewholder.Memberfirstname.Text = Memberaccountadapter[position].MemberFirstName;
            viewholder.MemberLastname.Text = Memberaccountadapter[position].MemberLastName;
            viewholder.MemberUsername.Text = Memberaccountadapter[position].MemberUserName;
            viewholder.MmeberEmail.Text = Memberaccountadapter[position].MemberEmail;
            viewholder.MemberContact.Text = Memberaccountadapter[position].MemberContact;
            viewholder.MemberFaculty.Text = Memberaccountadapter[position].MemberFaculty;
            viewholder.MemberAssociation.Text = Memberaccountadapter[position].MemberAssociation;
            viewholder.MemberPosition.Text = Memberaccountadapter[position].MemberPosition;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MemberProfileDetailsCard, parent, false);

            var viewsholders = new MemberProfileViewHolder(view, OnClick);

            return viewsholders;
        }
    }
    public class MemberItemClickArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
