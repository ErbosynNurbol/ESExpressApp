using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.ViewModels
{
    public partial class EditProfilePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        PersonModel person;

        private readonly ILocalizationResourceManager resourceManager;
        public EditProfilePageViewModel(ILocalizationResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            LoadData();
        }

        [RelayCommand]
        private void Retry()
        {
            CurrentState = States.Loading;
            LoadData();
        }

        [RelayCommand]
        public void Refresh()
        {
            IsRefreshing = true;
              LoadData();
            IsRefreshing = false;
        }

        [RelayCommand]
        public void DeleteAccount()
        {
            App.AlertSvc.ShowConfirmation(this.resourceManager["ls_Deleteaccount"], this.resourceManager["ls_Deleteafyd"], (result =>
            {
                if (result)
                {
                    AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/user/deleteaccount", null);
                    if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
                        {
                            AppSingleton.GetInstance().SetLoginInfo(null);
                            Preferences.Set("localLoginInfo", "");
                            App.Current.MainPage = new AppShell();
                        }
                        else
                        {
                            App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}", this.resourceManager["ls_Close"]);
                        }
                    }
                }
            }), this.resourceManager["ls_Delete"], this.resourceManager["ls_Close"]);
        }
        
        [RelayCommand]
        private void LoadData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                CurrentState = States.Offline;
                return;
            }
            if (IsBusy) return;
            IsBusy = true;
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/personinfo", null);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                Person = JsonHelper.DeserializeObject<PersonModel>(ajaxMsg.Data.ToString());
                CurrentState = States.Success;
            }
            else
            {
                CurrentState = States.Error;
            }
            IsBusy = false;
        }

        [RelayCommand]
        private void ChangePhoneNumber()
        {
            GoToAsync($"{nameof(ChangePhoneNumberPage)}");
        }

        [RelayCommand]
        private void Save()
        {
            var dicParameters = new Dictionary<string, string>
                    {
                      {"realName", Person.RealName??string.Empty},
                      {"city", Person.City??string.Empty},
                      {"whatsApp", Person.WhatsApp??string.Empty},
                      {"secondaryPhone", Person.SecondaryPhone??string.Empty} 
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/send/PersonInfo", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                if (ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
                {
                    LoginInfoModel loginInfo = AppSingleton.GetInstance().GetLoginInfo();
                    loginInfo.UserInfo.RealName = Person.RealName;
                    loginInfo.UserInfo.WhatsApp = Person.WhatsApp;
                    loginInfo.UserInfo.SecondaryPhone = Person.SecondaryPhone;
                    AppSingleton.GetInstance().SetLoginInfo(loginInfo);
                    Toast.Make(this.resourceManager["ls_SavedSuccessfully"]).Show();
                }
                else
                {
                     App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}", this.resourceManager["ls_Close"]);
                }
            }
            else
            {
                string entryName = string.Empty;
                switch (ajaxMsg?.Data)
                {
                    case "userName":
                        {
                            entryName = $"({this.resourceManager["ls_Yourname"]})";
                        }
                        break;
                    case "city":
                        {
                            entryName = $"({this.resourceManager["ls_District"]})";
                        }
                        break;
                }
                App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}{entryName}", this.resourceManager["ls_Close"]);
            }
        }
    }
}

