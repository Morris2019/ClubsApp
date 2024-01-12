using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;

namespace GTUCClubsApp
{
    public class CommentListener : Java.Lang.Object, IValueEventListener
    {
        public event EventHandler<CommentEvent> announcementevent;

        List<CommentModel> CommentList = new List<CommentModel>();

        public class CommentEvent : EventArgs
        {
            public List<CommentModel> EventComment { get; set; }
        }

        public void OnCancelled(DatabaseError error)
        {
            return;
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                CommentList.Clear();
                foreach (DataSnapshot AnnounModel in child)
                {
                    CommentModel announcemode = new CommentModel();

                    announcemode.CommenID = AnnounModel.Key;
                    announcemode.AnnouncementId = AnnounModel.Child("AnnouncementID").Value.ToString();
                    announcemode.MemberId = AnnounModel.Child("MemberUserId").Value.ToString();
                    announcemode.MemmberUsername = AnnounModel.Child("UserName").Value.ToString();
                    announcemode.MemberProfile = AnnounModel.Child("MemProfilePic").Value.ToString();
                    announcemode.announceComment = AnnounModel.Child("Comment").Value.ToString();
                    announcemode.DatePosted = AnnounModel.Child("DateCreated").Value.ToString();

                    CommentList.Add(announcemode);
                }
                announcementevent.Invoke(this, new CommentEvent { EventComment = CommentList });
            }
            else
            {
                return;
            }
        }
        public void CommentLoad()
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("AnnounceComment/");
            Query oderbydate = profileref.OrderByChild("AnnouncementIDs");
            oderbydate.AddValueEventListener(this);
        }
    }
}
