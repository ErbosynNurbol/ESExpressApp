using System;
using System.Reflection;
using CommunityToolkit.Mvvm.Messaging;
using Foundation;
using UserNotifications;

namespace ESExpressApp.Platforms.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;

            string navigationID = userInfo["navigationID"].ToString();
            Preferences.Set("navigationID", navigationID);

            WeakReferenceMessenger.Default.Send(new PushNotificationReceived("test"));
        }


        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.Banner | UNNotificationPresentationOptions.List);

            //completionHandler(UNNotificationPresentationOptions.Banner);
            //completionHandler(UNNotificationPresentationOptions.Alert);
        }

    }
}

