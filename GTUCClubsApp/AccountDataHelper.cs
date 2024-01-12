using System;
using Android.App;
using Android.Content;
using Firebase;
using Firebase.Database;

namespace GTUCClubsApp
{
    public static class AccountDataHelper
    {
        static ISharedPreferences UserDataStore = Application.Context.GetSharedPreferences("fenriesUserData", FileCreationMode.Private);

        public static FirebaseDatabase GetDatabase()
        {

            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase database;

            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApplicationId("1:678155026412:android:14b4c7270e6a73de03cac9")
                    .SetApiKey("AIzaSyBsR-7Zux9474SITBPnZdd-dLnS_jC58KM")
                    .SetDatabaseUrl("https://gtucclubsapp.firebaseio.com/")
                    .SetStorageBucket("gs://gtucclubsapp.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, option);
                database = FirebaseDatabase.GetInstance(app);
            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }

            return database;
        }
        public static string UserName()
        {
            string UserName = UserDataStore.GetString("UserName", "");
            return UserName;
        }
        public static string FirstName()
        {
            string FirstName = UserDataStore.GetString("FirstName", "");
            return FirstName;
        }
        public static string LastName()
        {
            string LastName = UserDataStore.GetString("LastName", "");
            return LastName;
        }

        public static string MemberAssociation()
        {
            string MemberAssociation = UserDataStore.GetString("MemberAssociation", "");
            return MemberAssociation;
        }
        public static string MemberEmail()
        {
            string MemberEmail = UserDataStore.GetString("MemberEmail", "");
            return MemberEmail;
        }
        public static string MemberProPic()
        {
            string MemberProPic = UserDataStore.GetString("MemberProPic", "");
            return MemberProPic;
        }
    }
}
