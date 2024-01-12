using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;

namespace GTUCClubsApp
{
    public class MemberProfileListener : Java.Lang.Object, IValueEventListener
    {
        List<MembersDataModel> Memberaccount = new List<MembersDataModel>();

        public event EventHandler<MemberProfileEvent> memberAccountretrieve;

        public class MemberProfileEvent : EventArgs
        {
            public List<MembersDataModel> memberaccountModels { get; set; }
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
                Memberaccount.Clear();
                foreach (DataSnapshot membersModel in child)
                {
                    MembersDataModel memberaccount = new MembersDataModel
                    {
                        MemberId = membersModel.Key,
                        MemberFirstName = membersModel.Child("FirstName").Value.ToString(),
                        MemberLastName = membersModel.Child("LastName").Value.ToString(),
                        MemberUserName = membersModel.Child("UserName").Value.ToString(),
                        MemberContact = membersModel.Child("UserContact").Value.ToString(),
                        MemberEmail = membersModel.Child("MemberEmail").Value.ToString(),
                        MemberAssociation = membersModel.Child("MemberAssociation").Value.ToString(),
                        MemberFaculty = membersModel.Child("Faculty").Value.ToString(),
                        MemberPosition = membersModel.Child("MemberPosition").Value.ToString(),
                        MemberProfileImages = membersModel.Child("MemberProPic").Value.ToString()
                    };

                    Memberaccount.Add(memberaccount);
                    
                }
                memberAccountretrieve.Invoke(this, new MemberProfileEvent { memberaccountModels = Memberaccount });
            }
            else
            {
                return;
            }
        }
        public void MemberProfilePic()
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("Users");
            Query dataOrderBy = profileref.OrderByKey().EqualTo(FirebaseAuth.Instance.CurrentUser.Uid.ToString());
            dataOrderBy.AddValueEventListener(this);
        }
        public void MemeberChatList()
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("Users");
            profileref.AddValueEventListener(this);
        }

    }
}