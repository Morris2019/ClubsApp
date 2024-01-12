using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GTUCClubsApp
{
    public class ChatViewAdater : RecyclerView.Adapter
    {
        List<MembersMessageModel> ChatModels = new List<MembersMessageModel>();
        public static int MemberRecieve = 0;
        public static int MemberSender = 1;

        public ChatViewAdater(List<MembersMessageModel> ChatModels)
        {
            this.ChatModels = ChatModels;
        }

        public override int ItemCount => ChatModels.Count;

        public class ChatViewHolder : RecyclerView.ViewHolder
        {
            public TextView SenderMessage;

           public ChatViewHolder (View itemView) : base(itemView)
            {
                SenderMessage = itemView.FindViewById<TextView>(Resource.Id.Messages);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            var viewholder = holder as ChatViewHolder;

            viewholder.SenderMessage.Text = ChatModels[position].UsersMessages;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == MemberRecieve)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MembermsgSender, parent, false);

                var viewsholders = new ChatViewHolder(view);

                return viewsholders;
            }
            else 
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MembermsgReciever, parent, false);

                var viewsholders = new ChatViewHolder(view);

                return viewsholders;
            }
        }
    }
}
