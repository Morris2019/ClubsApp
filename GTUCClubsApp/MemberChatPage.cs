using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Gms.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Firebase.Auth;
using Java.Util;
using Refractored.Controls;
using Xamarin.Essentials;
using Android.Support.V7.Widget;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

namespace GTUCClubsApp
{
    [Activity(Label = "MemberChatPage", Theme = "@style/AppTheme.NoActionBar")]
    public class MemberChatPage : AppCompatActivity, IOnCompleteListener
    {
        CircleImageView ProfilePicture;
        TextView Memberfirstname,MemberlastName;
        EditText Memberbermessagebox;
        FloatingActionButton SendMessage;
        RelativeLayout MemberChatPageRelative;
        RecyclerView ChatpageView;
        public string RecieverUserId { get; private set; }
        MemberMessageListeners MembersChats;
        List<MembersMessageModel> MembersMessages = new List<MembersMessageModel>();
        private MembersMessageModel UsersMessages;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MemberChatPage);
            // Create your application here
            AppCenter.Start("59d2dbe0-7ac4-4e84-8d83-3743ac8c0967",
                   typeof(Push) );
            //AppCenter.Start("59d2dbe0-7ac4-4e84-8d83-3743ac8c0967",
            //                   typeof(Analytics), typeof(Crashes));

            ProfilePicture = FindViewById<CircleImageView>(Resource.Id.MemberMessageUserProfile);
            Memberfirstname = FindViewById<TextView>(Resource.Id.MemberFirstNames);
            MemberlastName = FindViewById<TextView>(Resource.Id.MemberLastNames);
            MemberChatPageRelative = FindViewById<RelativeLayout>(Resource.Id.MemberChatPage);
            SendMessage = FindViewById<FloatingActionButton>(Resource.Id.UserSendFab);
            SendMessage.Click += MemberMesageSend;

            Memberbermessagebox = FindViewById<EditText>(Resource.Id.userMessage);
            ChatpageView = FindViewById<RecyclerView>(Resource.Id.MemberChatRecyclerView);

            RecieverUserId = Intent.GetStringExtra("RecieverUserId");
            Memberfirstname.Text = Intent.GetStringExtra("MemberFirstname");
            MemberlastName.Text = Intent.GetStringExtra("MemberLastname");

            var UserImageUrl = Intent.GetStringExtra("MemberProfileImages");
            if (UserImageUrl != null)
            {
                ImageService.Instance.LoadUrl(UserImageUrl).Retry(5, 200).Into(ProfilePicture);
            }

         LoadMembersMessages();
           // LoadMembersRecieverMessages();

        }
        public void LoadMemberMessages()
        {
            ChatpageView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(ChatpageView.Context));
            ChatViewAdater MessagesAdapter = new ChatViewAdater(MembersMessages);
            ChatpageView.SetAdapter(MessagesAdapter);
        }
        private void MemberMesageSend(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                if (string.IsNullOrEmpty(Memberbermessagebox.Text))
                {

                }
                else
                {
                    MemberSendMessage();
                }
            }
            else
            {
                Snackbar.Make(MemberChatPageRelative, "Please Check Your Network and Try Again.", Snackbar.LengthLong)
                .SetAction("OK", (view) => { })
                .Show();
            }

        }

        private void MemberSendMessage()
        {
            FirebaseUser user = FirebaseAuth.Instance.CurrentUser;
            string UserMsg = Memberbermessagebox.Text;
            string UserIdInsert = user.Uid;

            HashMap SendUserMsg = new HashMap();
            SendUserMsg.Put("Sender", UserIdInsert);
            SendUserMsg.Put("Reciever", RecieverUserId);
            SendUserMsg.Put("Message", UserMsg);
            SendUserMsg.Put("MsgDateTime", DateTime.Now.ToString());


            var UserDataBase = AccountDataHelper.GetDatabase().GetReference("Chats").Push();
            UserDataBase.SetValue(SendUserMsg).AddOnCompleteListener(this, this);
        
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Memberbermessagebox.Text = string.Empty;
                return;
            }
            else
            {
                return;
            }
        }
        public void LoadMembersMessages()
        {
            MembersChats = new MemberMessageListeners();
            MembersChats.CreateChats(RecieverUserId);
            MembersChats.messageretrieve += ChatRetrieveEvent;
        }

        private void ChatRetrieveEvent(object sender, MemberMessageListeners.MemberMessageEvent e)
        {
            MembersMessages = e.MessageModels;
            LoadMemberMessages();
        }
        public void LoadMembersRecieverMessages()
        {
            MembersChats = new MemberMessageListeners();
            MembersChats.CreateChatsReciever();
            MembersChats.messageretrieve += ChatRetrieveEvent;
        }
    }
}
