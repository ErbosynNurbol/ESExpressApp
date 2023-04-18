using System.Threading;
using LocalizationResourceManager.Maui;
using Microsoft.Maui;
using static System.Net.Mime.MediaTypeNames;

namespace ESExpressApp.Views;

public partial class ChangePasswordPage : ContentPage
{
    private BorderlessEntry entryOldPassword;
    private BorderlessEntry entryPassword;
    private BorderlessEntry entryConfirmPassword;
    private BorderlessEntry entryVerifyCode_1;
    private BorderlessEntry entryVerifyCode_2;
    private BorderlessEntry entryVerifyCode_3;
    private BorderlessEntry entryVerifyCode_4;
    private BorderlessEntry entryVerifyCode_5;
    private bool isCheckingVeriftyCode = false;
    private bool isAppearing = false;
    private readonly ILocalizationResourceManager resourceManager;
    public ChangePasswordPage(ILocalizationResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
        InitializeComponent();
        entryOldPassword = oldPasswordEntryControl.FindByName<BorderlessEntry>("customEntry");
        entryPassword = passwordEntryControl.FindByName<BorderlessEntry>("customEntry");
        entryConfirmPassword = confirmPasswordEntryControl.FindByName<BorderlessEntry>("customEntry");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isAppearing)
        {
            isAppearing = true;
            entryOldPassword.Completed += (object sender, EventArgs e) =>
            {
                entryConfirmPassword.Focus();
            };
            entryPassword.Completed += (object sender, EventArgs e) =>
            {
                entryConfirmPassword.Focus();
            };
            entryConfirmPassword.Completed += (object sender, EventArgs e) =>
            {
                entryConfirmPassword.Unfocus();
            };
        }
    }

    private void btnChangePassword_Clicked(object sender, EventArgs e)
    {
        var dicParameters = new Dictionary<string, string>
                    {
                      {"oldPassword", entryOldPassword.Text},
                      {"password",entryPassword.Text},
                      {"confirmPassword", entryConfirmPassword.Text}
                    };
        AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/user/ChangePassword", dicParameters);
        if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
        {
            entryOldPassword.Text = string.Empty;
            entryPassword.Text = string.Empty;
            entryConfirmPassword.Text = string.Empty;
            Toast.Make(ajaxMsg.Message).Show().ContinueWith((x) => Shell.Current.GoToAsync("..", true));
        }
        else
        {
            string entryName = string.Empty;
            switch (ajaxMsg?.Data)
            {
                case "oldPassword":
                    {
                        entryName = $"({this.resourceManager["ls_Oldpassword"]})";
                    }
                    break;
                case "password":
                    {
                        entryName = $"({this.resourceManager["ls_Newpassword"]})";
                    }
                    break;
                case "confirmPassword":
                    {
                        entryName = $"({this.resourceManager["ls_ConfirmPassword"]})";
                    }
                    break;
            }
            App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}{entryName}", this.resourceManager["ls_Close"]);
        }
    }
}
