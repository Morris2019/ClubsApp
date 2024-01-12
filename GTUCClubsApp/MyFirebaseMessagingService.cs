using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;

using Firebase.Messaging;

namespace GTUCClubsApp
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        private readonly string Notification_Channel_ID = "fenries.com.gtucclubsapp" ;

        public override void OnMessageReceived(RemoteMessage message)
        {
            if (message.Data.GetEnumerator().MoveNext())
            {
               SendNotification(message.GetNotification().Title, message.GetNotification().Body);
            }
            else
            {
               SendNotification(message.Data);
            }
            Log.Debug(TAG, "From: " + message.From);

            var body = message.GetNotification().Body;
            Log.Debug(TAG, "Notification Message Body: " + body);
            SendNotification(body, message.Data);
        }

        private void SendNotification(IDictionary<string, string> data)
        {
            string title, body;
            data.TryGetValue("title", out title);
            data.TryGetValue("body", out body);

            SendNotification(title, body);
        }

        private void SendNotification(string title, string body)
        {
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(Notification_Channel_ID, "Notification Channel ID",
                    Android.App.NotificationImportance.Default);

                notificationChannel.Description = "Club'sApp";
                notificationChannel.EnableLights(true);
                notificationChannel.LightColor = Color.Blue;
                notificationChannel.SetVibrationPattern(new long[] { 0, 100, 500, 1000 });
                notificationManager.CreateNotificationChannel(notificationChannel);
            }
            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this, Notification_Channel_ID);
            notificationBuilder.SetAutoCancel(true)
                .SetDefaults(-1)
                .SetWhen(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                .SetContentTitle(title)
                .SetContentText(body)
                .SetContentInfo("info");

            notificationManager.Notify(new Random().Next(), notificationBuilder.Build());
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(PROMainPage));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this, PROMainPage.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, PROMainPage.CHANNEL_ID)
                                      .SetSmallIcon(Resource.Drawable.outline_add_a_photo_black_24dp)
                                      .SetContentTitle("FCM Message")
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(PROMainPage.NOTIFICATION_ID, notificationBuilder.Build());
        }
    }
}
