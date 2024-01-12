using System;
using Android.App;
using Android.Content;
using Firebase.Auth;
using Firebase.Database;

namespace GTUCClubsApp
{
    public class UserProfileListener : Java.Lang.Object, IValueEventListener
    {
        ISharedPreferences UserDataStore = Application.Context.GetSharedPreferences("fenriesUserData", FileCreationMode.Private);
        ISharedPreferencesEditor UserdataEditor;


        public UserProfileListener()
        {
        }

        public void OnCancelled(DatabaseError error)
        {
           
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if(snapshot.Value != null)
            {
                string FirstName, LastName, MemberAssociation, MemberEmail, MemberProPic, UserName, MemberUserId;

                FirstName = (snapshot.Child("FirstName") != null) ? snapshot.Child("FirstName").Value.ToString() : "";
                LastName = (snapshot.Child("LastName") != null) ? snapshot.Child("LastName").Value.ToString() : "";
                MemberAssociation = (snapshot.Child("MemberAssociation") != null) ? snapshot.Child("MemberAssociation").Value.ToString() : "";
                MemberEmail = (snapshot.Child("MemberEmail") != null) ? snapshot.Child("MemberEmail").Value.ToString() : "";
                MemberProPic = (snapshot.Child("MemberProPic") != null) ? snapshot.Child("MemberProPic").Value.ToString() : "";
                UserName = (snapshot.Child("UserName") != null) ? snapshot.Child("UserName").Value.ToString() : "";
                MemberUserId = (snapshot.Child("MemberUserId") != null) ? snapshot.Child("MemberUserId").Value.ToString() : "";


                UserdataEditor.PutString("FirstName", FirstName);
                UserdataEditor.PutString("LastName", LastName);
                UserdataEditor.PutString("MemberAssociation", MemberAssociation);
                UserdataEditor.PutString("MemberEmail", MemberEmail);
                UserdataEditor.PutString("MemberProPic", MemberProPic);
                UserdataEditor.PutString("UserName", UserName);

                UserdataEditor.Apply();
            }
            
        }
        public void UserCreate()
        {
            UserdataEditor = UserDataStore.Edit();

            FirebaseDatabase UserAccounts = AccountDataHelper.GetDatabase();
            string UserID = FirebaseAuth.Instance.CurrentUser.Uid.ToString();
            DatabaseReference profileref = AccountDataHelper.GetDatabase().GetReference("Users/" + UserID);
            profileref.AddValueEventListener(this);
        }
    }
}
