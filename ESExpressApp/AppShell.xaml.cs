
namespace ESExpressApp;

public partial class AppShell : Shell
{
    public const string localPassword = "ZESEXPRESS2023@";
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
        
         var dicParameters = new Dictionary<string, string>
                    {
                      {"phone", phone},
                      {"password", password},
                      {"countryId", countryId.ToString()},
                      //{"AndroidToken", DeviceInfo.Platform == DevicePlatform.Android? CrossFirebasePushNotification.Current.Token: ""},
                      //{"IOSToken", DeviceInfo.Platform == DevicePlatform.iOS ? CrossFirebasePushNotification.Current.Token : ""}
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
