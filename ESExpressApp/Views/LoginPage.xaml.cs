using ESExpressApp.Handlers;
using ESExpressApp.Helpers;
using ESExpressApp.Models;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.Views;

public partial class LoginPage : ContentPage
{
    private BorderlessEntry entryPhone;
    private BorderlessEntry entryPassword;
    private readonly ILocalizationResourceManager resourceManager;
    public LoginPage(ILocalizationResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
        InitializeComponent();
        entryPhone = phoneEntryControl.FindByName<BorderlessEntry>("customEntry");
        if (entryPhone != null)
        {
            entryPhone.Completed += EntryPhone_Completed;
        }
        entryPassword = passwordEntryControl.FindByName<BorderlessEntry>("customEntry");
        if (entryPassword != null)
        {
            entryPassword.Completed += EntryPassword_Completed;
        }
    }

    private void EntryPhone_Completed(object sender, EventArgs e)
    {
        if (!entryPhone.Placeholder.Equals(entryPhone.Text))
        {
            entryPassword.Focus();
        }
    }
    private void EntryPassword_Completed(object sender, EventArgs e)
    {
        Login(entryPhone.Text, entryPassword.Text, 1);
    }
    private void btnLogin_Clicked(object sender, EventArgs e)
    {
        btnLogin.IsEnabled = false;
        Login(entryPhone.Text, entryPassword.Text, 1);
        btnLogin.IsEnabled = true;
    }

    private void Login(string phone, string password, int countryId)
    {
        if (AppShell.Login(phone, password, countryId, out string message))
        {
            string localLoginInfoStr = JsonHelper.SerializeObject(new LocalLoginInfoModel()
            {
                Phone = phone,
                Password = password,
                CountryId = countryId
            });
            localLoginInfoStr = AESHelper.EncryptText(localLoginInfoStr, AppShell.localPassword);
            Preferences.Set("localLoginInfo", localLoginInfoStr);
            Shell.Current.GoToAsync("..");
        }
        else
        {
            AppShell.Current.DisplayAlert(this.resourceManager["ls_Warning"], message, this.resourceManager["ls_Close"]);
            if (string.IsNullOrEmpty(phone)|| phone.Equals("+7"))
            {
                entryPhone.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                entryPassword.Focus();
                return;
            }
        }
    }

    void Close_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		Shell.Current.GoToAsync("..");
    }

    void Register_Clicked(System.Object sender, System.EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
    }

    void btnRecovery_Clicked(System.Object sender, System.EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(RecoverPage)}");
    }
}