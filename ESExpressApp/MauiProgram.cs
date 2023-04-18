using CommunityToolkit.Maui;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;

namespace ESExpressApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();

           builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseLocalizationResourceManager(settings =>
            {
                settings.AddResource(Resources.Translations.AppResources.ResourceManager);
                settings.RestoreLatestCulture(true);
            })
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-brands-400.ttf", "fbrands");
                fonts.AddFont("fa-duotone-900.ttf", "fduotone");
                fonts.AddFont("fa-light-300.ttf", "flight");
                fonts.AddFont("fa-regular-400.ttf", "fregular");
                fonts.AddFont("fa-solid-900.ttf", "fsolid");
            });

        #if DEBUG
          builder.Logging.AddDebug();
        #endif
        builder.Services.AddSingleton<IAlertService, AlertService>();

        builder.Services.AddTransient<Views.HomePage>();
        builder.Services.AddTransient<ViewModels.HomePageViewModel>();

        builder.Services.AddTransient<Views.CalculatorPage>();
        builder.Services.AddTransient<ViewModels.CalculatorPageViewModel>();

        builder.Services.AddTransient<Views.LangugePopupPage>();
        builder.Services.AddTransient<ViewModels.LangugePopupPageViewModel>();

        builder.Services.AddTransient<Views.ProfilePage>();
        builder.Services.AddTransient<ViewModels.ProfilePageViewModel>();

        builder.Services.AddTransient<Views.LoginPage>();
        builder.Services.AddTransient<Views.RegisterPage>();
        builder.Services.AddTransient<Views.RecoverPage>();

        builder.Services.AddTransient<Views.NotshippedListPage>();
        builder.Services.AddTransient<ViewModels.NotshippedListPageViewModel>();

        builder.Services.AddTransient<Views.IntransitListPage>();
        builder.Services.AddTransient<ViewModels.IntransitListPageViewModel>();

        builder.Services.AddTransient<Views.ArrivedAtWarehouseListPage>();
        builder.Services.AddTransient<ViewModels.ArrivedAtWarehouseListPageViewModel>();

        builder.Services.AddTransient<Views.TransactionCompletedListPage>();
        builder.Services.AddTransient<ViewModels.TransactionCompletedListPageViewModel>();

        builder.Services.AddTransient<Views.PersonWaybillPage>();
        builder.Services.AddTransient<ViewModels.PersonWaybillPageViewModel>();

        builder.Services.AddTransient<Views.EditProfilePage>();
        builder.Services.AddTransient<ViewModels.EditProfilePageViewModel>();

        builder.Services.AddTransient<Views.ChangePhoneNumberPage>();
        builder.Services.AddTransient<Views.ChangePasswordPage>();

        builder.Services.AddTransient<Views.WaybillNumberPage>();
        builder.Services.AddTransient<ViewModels.WaybillNumberPageViewModel>();

        return builder.Build();
	}
}

