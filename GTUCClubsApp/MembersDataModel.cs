using System;
namespace GTUCClubsApp
{
    public class MembersDataModel
    {
        public string MemberId { get; set; }
        public string MemberProfileImages { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberUserName { get; set; }
        public string MemberContact { get; set; }
        public string MemberEmail { get; set; }
        public string MemberFaculty { get; set; }
        public string MemberAssociation { get; set; }
        public string MemberPosition { get; set; }
        public string UserDateCreated { get; set; }

        public MembersDataModel()
        {
        }
    }
}
