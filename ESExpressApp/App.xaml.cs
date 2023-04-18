global using CommunityToolkit.Maui.Alerts;
global using ESExpressApp.Models;
global using ESExpressApp.Helpers;
global using ESExpressApp.Handlers;
global using ESExpressApp.ViewModels;
global using ESExpressApp.Views;
global using ESExpressApp.Services;

using Microsoft.Maui.Platform;
namespace ESExpressApp;

public partial class App : Application
{
    public static IServiceProvider Services;
    public static IAlertService AlertSvc;
    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Services = provider;
        AlertSvc = Services.GetService<IAlertService>();

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
                 handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                 handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
            }
        });

        MainPage = new AppShell(); //new EditProfilePage(new EditProfilePageViewModel()); 
    }
}

