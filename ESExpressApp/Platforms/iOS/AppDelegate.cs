using ESExpressApp.Platforms.iOS;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace ESExpressApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        Firebase.Core.App.Configure();

        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            var authOption = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

            UNUserNotificationCenter.Current.RequestAuthorization(authOption, (granted, error) =>
            {

            });

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            Messaging.SharedInstance.AutoInitEnabled = true;
            Messaging.SharedInstance.Delegate = this;
        }
        UIApplication.SharedApplication.RegisterForRemoteNotifications();

        return base.FinishedLaunching(application, launchOptions);
    }

    [Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging message, string regToken)
    {
        if (Preferences.ContainsKey("deviceToken"))
        {
            Preferences.Remove("deviceToken");
        }
        Preferences.Set("deviceToken", regToken);
    }
}

