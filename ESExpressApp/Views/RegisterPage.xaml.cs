using ESExpressApp.Handlers;
using ESExpressApp.Models;
using ESExpressApp.Helpers;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.Views;

public partial class RegisterPage : ContentPage
{
    private BorderlessEntry entryPhone;
    private BorderlessEntry entryUserName;
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
    public RegisterPage(ILocalizationResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
		InitializeComponent();
        entryPhone = phoneEntryControl.FindByName<BorderlessEntry>("customEntry");
        entryUserName = userNameEntryControl.FindByName<BorderlessEntry>("customEntry");
        entryPassword = passwordEntryControl.FindByName<BorderlessEntry>("customEntry");
        entryConfirmPassword = confirmPasswordEntryControl.FindByName<BorderlessEntry>("customEntry");
        entryVerifyCode_1 = verifyCodeTemplate.FindByName<BorderlessEntry>("entryVerifyCode_1");
        entryVerifyCode_2 = verifyCodeTemplate.FindByName<BorderlessEntry>("entryVerifyCode_2");
        entryVerifyCode_3 = verifyCodeTemplate.FindByName<BorderlessEntry>("entryVerifyCode_3");
        entryVerifyCode_4 = verifyCodeTemplate.FindByName<BorderlessEntry>("entryVerifyCode_4");
        entryVerifyCode_5 = verifyCodeTemplate.FindByName<BorderlessEntry>("entryVerifyCode_5");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isAppearing)
        {
            isAppearing = true;
            entryVerifyCode_5.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                CompletedOrTextChangedCheckVeriftyCode();
            };
            entryVerifyCode_5.Completed += (object sender, EventArgs e) =>
            {
                CompletedOrTextChangedCheckVeriftyCode();
            };
            entryUserName.Completed += (object sender, EventArgs e) =>
            {
                entryPassword.Focus();
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
    private void CompletedOrTextChangedCheckVeriftyCode()
    {
        if (!string.IsNullOrEmpty(entryVerifyCode_1.Text) &&
            !string.IsNullOrEmpty(entryVerifyCode_2.Text) &&
            !string.IsNullOrEmpty(entryVerifyCode_3.Text) &&
            !string.IsNullOrEmpty(entryVerifyCode_4.Text) &&
            !string.IsNullOrEmpty(entryVerifyCode_5.Text))
        {
            btnCheckVeriftyCode.IsEnabled = true;
            CheckVeriftyCode();
        }
        else
        {
            btnCheckVeriftyCode.IsEnabled = false;
        }
    }
    private void btnGetVerificationCode_Clicked(object sender, System.EventArgs e)
    {
        Button button = sender as Button;
        button.IsEnabled = false;
        string phone = entryPhone.Text;
        var dicParameters = new Dictionary<string, string>
                    {
                      {"phone", phone},
                      {"type", "register"},
                      {"countryId", "1"}
                    };
        AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/user/SendPhoneVerifyCode", dicParameters);
        if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
        {
            lblPhone.Text = this.resourceManager["ls_Etcfsstphone"].Replace("{phone}", phone);
            UpdateResendTime(60);
            btnResendcode.IsVisible = false; //Resend button
            entryVerifyCode_1.Focus();//Focus Verify Code
            slFirstStep.IsVisible = false;
            slSecondStep.IsVisible = true;
            slThirdStep.IsVisible = false;
        }
        else
        {
            if (string.IsNullOrEmpty(phone))
            {
                entryPhone.Focus();
            }
            AppShell.Current.DisplayAlert(this.resourceManager["ls_Warning"], ajaxMsg?.Message, this.resourceManager["ls_Close"]);
        }
        button.IsEnabled = true;
    }
    private void UpdateResendTime(int time)
    {
        Task.Run(() =>
        {
            if (time <= 0)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    lblResendTime.IsVisible = false;
                    btnResendcode.IsVisible = true;
                });
                return;
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                lblResendTime.IsVisible = true;
                lblResendTime.Text = this.resourceManager["ls_Ycgancits"].Replace("{time}", time.ToString());
            });
            Thread.Sleep(1000);
            if (time > 0)
            {
                UpdateResendTime(--time);
            }
        });
    }

    private void CheckVeriftyCode()
    {
        if (isCheckingVeriftyCode) return;
        isCheckingVeriftyCode = true;
        string phone = entryPhone.Text;
        string veriftyCode = String.Concat(entryVerifyCode_1.Text, entryVerifyCode_2.Text, entryVerifyCode_3.Text, entryVerifyCode_4.Text, entryVerifyCode_5.Text);
        var dicParameters = new Dictionary<string, string>
                    {
                      {"phone", phone},
                      {"type", "register"},
                      {"veriftyCode", veriftyCode},
                      {"countryId", "1"}
                    };
        AjaxMsgModel ajaxMsg = Helpers.APIHelper.Query($"/api/user/CheckPhoneVerifyCode", dicParameters);
        if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
        {
            slFirstStep.IsVisible = false;
            slSecondStep.IsVisible = false;
            slThirdStep.IsVisible = true;
            entryUserName.Focus();
        }
        else
        {
            AppShell.Current.DisplayAlert(this.resourceManager["ls_Warning"], ajaxMsg.Message, this.resourceManager["ls_Close"]);
        }
        isCheckingVeriftyCode = false;
    }

    private void btnCheckVeriftyCode_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        button.IsEnabled = false;
        CheckVeriftyCode();
        button.IsEnabled = true;
    }

    private void btnRegister_Clicked(object sender, EventArgs e)
    {
        string phone = entryPhone.Text;
        string veriftyCode = String.Concat(entryVerifyCode_1.Text, entryVerifyCode_2.Text, entryVerifyCode_3.Text, entryVerifyCode_4.Text, entryVerifyCode_5.Text);
        string userName = entryUserName.Text;
        string password = entryPassword.Text;
        string confirmPassword = entryConfirmPassword.Text;
        var dicParameters = new Dictionary<string, string>
                    {
                      {"phone", phone},
                      {"type", "register"},
                      {"veriftyCode", veriftyCode},
                      {"userName", userName},
                      {"password", password},
                      {"confirmPassword", confirmPassword},
                      {"countryId", "1"}
                    };
        AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/user/register", dicParameters);
        if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
        {
            Toast.Make(ajaxMsg.Message).Show().ContinueWith((x) => Shell.Current.GoToAsync("..", true));
        }
        else
        {
            AppShell.Current.DisplayAlert(this.resourceManager["ls_Warning"], ajaxMsg.Message, this.resourceManager["ls_Close"]);
        }
    }
    void Close_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
}
