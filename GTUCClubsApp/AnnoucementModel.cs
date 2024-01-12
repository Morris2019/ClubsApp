using System;
namespace GTUCClubsApp
{
    public class AnnoucementModel
    {
        public string AnnoucementId { get; set; }
        public string AnnounceMemberId { get; set; }
        public string AnnounceImage { get; set; }
        public string MemberAssociation { get; set; }
        public string AnnounceCaption { get; set; }
        public string EventDiscrip { get; set; }
        public string Faculty { get; set; }
        public string Venue { get; set; }
        public string Event_Date {get;set;}
        public string DateCreated { get; set; }

       
        public AnnoucementModel()
        {
        }
    }
}
