using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;

namespace GTUCClubsApp
{
    public class MemberMessageListeners : Java.Lang.Object, IValueEventListener
    {
        List<MembersMessageModel> membermessagemodel = new List<MembersMessageModel>();

        public event EventHandler<MemberMessageEvent> messageretrieve;

        public class MemberMessageEvent : EventArgs
        {
            public List<MembersMessageModel> MessageModels { get; set; }
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
                membermessagemodel.Clear();
                foreach (DataSnapshot membersModel in child)
                {
                    MembersMessageModel messageaccount = new MembersMessageModel();

                    messageaccount.MessageId = membersModel.Key;
                    messageaccount.MsgRecieverId = membersModel.Child("Reciever").Value.ToString();
                    messageaccount.MsgSenderId = membersModel.Child("Sender").Value.ToString();
                    messageaccount.MsgDate = membersModel.Child("MsgDateTime").Value.ToString();
                    messageaccount.UsersMessages = membersModel.Child("Message").Value.ToString();

                    membermessagemodel.Add(messageaccount);
                }
                messageretrieve.Invoke(this, new MemberMessageEvent { MessageModels = membermessagemodel });
            }
            else
            {
                return;
            }
        }
        public void CreateChats(string key)
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("Chats/" +key);
            Query dataOrderBy = profileref.OrderByChild("Sender").EqualTo(FirebaseAuth.Instance.CurrentUser.Uid.ToString());
            dataOrderBy.AddValueEventListener(this);
        }
        public void CreateChatsReciever()
        {
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("Chats");
            Query dataOrderBy = profileref.OrderByChild("Reciever");
            dataOrderBy.AddValueEventListener(this);
        }
    }
}
