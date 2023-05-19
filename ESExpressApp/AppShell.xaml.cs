
using CommunityToolkit.Mvvm.Messaging;

namespace ESExpressApp;

public partial class AppShell : Shell
{
    public const string localPassword = "ZESEXPRESS2023@";
    private string _deviceToken;
    public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(ArticleViewPage), typeof(ArticleViewPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(RecoverPage), typeof(RecoverPage));
        Routing.RegisterRoute(nameof(NotshippedListPage), typeof(NotshippedListPage));
        Routing.RegisterRoute(nameof(IntransitListPage), typeof(IntransitListPage));
        Routing.RegisterRoute(nameof(ArrivedAtWarehouseListPage), typeof(ArrivedAtWarehouseListPage));
        Routing.RegisterRoute(nameof(TransactionCompletedListPage), typeof(TransactionCompletedListPage));
        Routing.RegisterRoute(nameof(PersonWaybillPage), typeof(PersonWaybillPage));
        Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
        Routing.RegisterRoute(nameof(ChangePhoneNumberPage), typeof(ChangePhoneNumberPage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(UploadAvatarPage), typeof(UploadAvatarPage));
        Routing.RegisterRoute(nameof(WaybillNumberPage), typeof(WaybillNumberPage));
        Task.Run(() =>
        {
            TryLocalLogin(out LoginInfoModel loginInfo);
        });

        WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
        {
            NavigateToPage();
        });
        NavigateToPage();
    }

    public void SwitchtoTab(int tabIndex)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            switch (tabIndex)
            {
                case 0: shellTabBar.CurrentItem = tabHome; break;
                case 1: shellTabBar.CurrentItem = tabCalculator; break;
                case 2: shellTabBar.CurrentItem = tabHelp; break;
                case 3: shellTabBar.CurrentItem = tabProfile; break;
            };
        });
    }

    private void NavigateToPage()
    {
        if (Preferences.ContainsKey("navigationID"))
        {
            if(int.TryParse(Preferences.Get("navigationID", "0"),out int navigationID))
            {
                if (navigationID <= 4) //Shell Page
                {
                    SwitchtoTab(navigationID);
                }
            }
            Preferences.Remove("navigationID");
        }
    }

    #region Login  +Login(string phone, string password, int countryId, out string message)

    private void TryLocalLogin(out LoginInfoModel loginInfo)
    {
        loginInfo = null;
        string localLoginInfoStr = Preferences.Get("localLoginInfo", "");
        if (!string.IsNullOrEmpty(localLoginInfoStr))
        {
            localLoginInfoStr = AESHelper.DecryptText(localLoginInfoStr, localPassword);
            LocalLoginInfoModel localLoginInfo = JsonHelper.DeserializeObject<LocalLoginInfoModel>(localLoginInfoStr);
            if (localLoginInfo != null && Login(localLoginInfo.Phone, localLoginInfo.Password, localLoginInfo.CountryId, out string message))
            {
                loginInfo = AppSingleton.GetInstance().GetLoginInfo();
            }
            else
            {
                Preferences.Set("localLoginInfo", "");
            }
        }
    }

    public static bool Login(string phone, string password, int countryId, out string message)
    {
        message = string.Empty;

        string deviceToken = Preferences.Get("deviceToken", string.Empty);
         var dicParameters = new Dictionary<string, string>
                    {
                      {"phone", phone},
                      {"password", password},
                      {"countryId", countryId.ToString()},
                      {"androidToken", DeviceInfo.Current.Platform  == DevicePlatform.Android? deviceToken: ""},
                      {"iOSToken", DeviceInfo.Current.Platform ==  DevicePlatform.iOS ? deviceToken : ""}
                    };
        AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/user/login", dicParameters);
        if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
        {
            LoginInfoModel loginInfo = JsonHelper.DeserializeObject<LoginInfoModel>(ajaxMsg.Data.ToString());
            AppSingleton.GetInstance().SetLoginInfo(loginInfo);
            return true;
        }
        else
        {
            message = ajaxMsg?.Message;
        }
        return false;
    }
    #endregion
}
