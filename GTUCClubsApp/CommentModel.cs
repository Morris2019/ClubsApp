using System;
namespace GTUCClubsApp
{
    public class CommentModel
    {
        public string CommenID { get; set; }
        public string announceComment { get; set; }
        public string DatePosted { get; set; }
        public string MemberId { get; set; }
        public string AnnouncementId { get; set; }
        public string MemmberUsername {get;set;}
        public string Memberlname { get; set; }
        public string MemberProfile { get; set; }

        public CommentModel( )
        {
           
        }
    }
}
