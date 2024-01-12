using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;

namespace GTUCClubsApp
{
    public class AnnouncementListeners : Java.Lang.Object, IValueEventListener
    {
        List<AnnoucementModel> announcemodel = new List<AnnoucementModel>();

        public event EventHandler<AnnouncementEvent> announcementevent;

        public class AnnouncementEvent : EventArgs
        {
            public List<AnnoucementModel> Announceeventmodel { get; set; }
        }
        public AnnouncementListeners()
        {
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
                announcemodel.Clear();
                foreach (DataSnapshot AnnounModel in child)
                {
                    AnnoucementModel announcemode = new AnnoucementModel();

                    announcemode.AnnoucementId = AnnounModel.Key;
                    announcemode.AnnounceCaption = AnnounModel.Child("AnnounceCaption").Value.ToString();
                    announcemode.AnnounceMemberId = AnnounModel.Child("MemberUserId").Value.ToString();
                    announcemode.EventDiscrip = AnnounModel.Child("EventDiscription").Value.ToString();
                    announcemode.AnnounceImage = AnnounModel.Child("AnnounceImage").Value.ToString();
                    announcemode.Event_Date = AnnounModel.Child("Event_Date").Value.ToString();
                    announcemode.Faculty = AnnounModel.Child("Faculty").Value.ToString();
                    announcemode.MemberAssociation = AnnounModel.Child("MemberAssociation").Value.ToString();
                    announcemode.Venue = AnnounModel.Child("Venue").Value.ToString();
                    announcemode.DateCreated = AnnounModel.Child("DateCreated").Value.ToString();
                    announcemodel.Add(announcemode);
                }
                announcementevent.Invoke(this, new AnnouncementEvent { Announceeventmodel = announcemodel });
            }
            else
            {
                return;
            }
        }
        public void AnnouncementLoad()
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("ClubsAnnouncement");
            Query oderbydate = profileref.OrderByChild("DateCreated");
            oderbydate.AddValueEventListener(this);
        }
        public void DeleteEvent(string key)
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("ClubsAnnouncement/" + key);
            profileref.RemoveValue();
        }
    }
}
