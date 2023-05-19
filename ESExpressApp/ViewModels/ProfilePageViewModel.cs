using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ESExpressApp.Models;
using ESExpressApp.Helpers;
using ESExpressApp.Views;
using CommunityToolkit.Maui.Views;
using LocalizationResourceManager.Maui;
using System.Globalization;

namespace ESExpressApp.ViewModels
{
	public partial class ProfilePageViewModel : BaseViewModel
    {

        [ObservableProperty]
        ObservableCollection<ProfileMenuItemModel> mainMenuList = new ObservableCollection<ProfileMenuItemModel>();
        [ObservableProperty]
        ProfileMenuItemModel mainMenuSelected;
        [ObservableProperty]
        ObservableCollection<ProfileMenuItemModel> settingsMenuList = new ObservableCollection<ProfileMenuItemModel>();
        [ObservableProperty]
        ProfileMenuItemModel settingsMenuSelected;
        [ObservableProperty]
        PersonModel person;
        [ObservableProperty]
        ObservableCollection<ShippingMethodModel> shippingMethodList = new ObservableCollection<ShippingMethodModel>();
        [ObservableProperty]
        ShippingMethodModel selectedShippingMethod;
        [ObservableProperty]
        string shippingMethodBackgroudImage;
        private readonly ILocalizationResourceManager resourceManager;
        public ProfilePageViewModel(ILocalizationResourceManager resourceManager)
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
        private void MainMenuListChanged()
        {
            if (AppSingleton.GetInstance().GetLoginInfo() != null)
            {
                if (MainMenuSelected != null)
                {
                    switch (MainMenuSelected.Key)
                    {
                        case ProfileMenuStatus.Notshipped:
                            {
                               GoToAsync($"{nameof(NotshippedListPage)}");
                            }
                            break;
                        case ProfileMenuStatus.Intransit:
                            {
                                GoToAsync($"{nameof(IntransitListPage)}");
                            }
                            break;
                        case ProfileMenuStatus.Arrivedatwarehouse:
                            {
                                GoToAsync($"{nameof(ArrivedAtWarehouseListPage)}");
                            }
                            break;
                        case ProfileMenuStatus.TransactionCompleted:
                            {
                                GoToAsync($"{nameof(TransactionCompletedListPage)}");
                            }
                            break;
                    }
                }
            }
            else
            {
                ShowLoginPage();
            }
            MainMenuSelected = null;
        }

        [RelayCommand]
        private void SettingsMenuListChanged()
        {
            if (AppSingleton.GetInstance().GetLoginInfo() != null)
            {
                if (SettingsMenuSelected != null)
                {
                    switch (SettingsMenuSelected.Key)
                    {
                        case ProfileMenuStatus.Language:
                            {
                                Shell.Current.CurrentPage.ShowPopup(new LangugePopupPage(this.resourceManager));
                            }
                            break;
                        case ProfileMenuStatus.PersonalInfo:
                            {
                                GoToAsync($"{nameof(EditProfilePage)}");
                            }
                            break;
                        case ProfileMenuStatus.ChangePassword:
                            {
                                GoToAsync($"{nameof(ChangePasswordPage)}");
                            }
                            break;
                    }
                }
            }
            else
            {
                ShowLoginPage();
            }
            SettingsMenuSelected = null;
        }

        [RelayCommand]
        private void ShowLoginPage()
        {
            GoToAsync($"{nameof(LoginPage)}");
        }

        [RelayCommand]
        private void Signout()
        {
            AppSingleton.GetInstance().SetLoginInfo(null);
            Preferences.Set("localLoginInfo", "");
            App.Current.MainPage = new AppShell();
        }

        [RelayCommand]
        private async void Copy()
        {
            if (AppSingleton.GetInstance().GetLoginInfo() != null)
            {
                await Clipboard.Default.SetTextAsync(SelectedShippingMethod.WarehouseAddress);
                await Toast.Make(this.resourceManager["ls_Copysuccessfully"]).Show();
            }
            else
            {
                ShowLoginPage();
            }
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
            Person = AppSingleton.GetInstance().GetLoginInfo()?.UserInfo;
            if (MainMenuList == null || MainMenuList.Count() ==0)
            {
                MainMenuList = GetDefaultMainMenuList();
            }
            if (SettingsMenuList == null || SettingsMenuList.Count() == 0)
            {
                SettingsMenuList = GetDefaultSettingsMenuList();
            }
            Task.Run(() =>
            {
                AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/PersonPackageCount", null);
                if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    Dictionary<string, int> divDic = JsonHelper.DeserializeObject<Dictionary<string, int>>(ajaxMsg.Data.ToString());
                    foreach (var item in MainMenuList)
                    {
                        if (divDic.ContainsKey(item.Key.ToString().ToLower()))
                        {
                            item.Count = divDic[item.Key.ToString().ToLower()];
                        }
                    }
                }
                ajaxMsg = APIHelper.Query($"/api/query/ShippingMethodList", null);
                if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    ShippingMethodList = JsonHelper.DeserializeObject<ObservableCollection<ShippingMethodModel>>(ajaxMsg.Data.ToString());
                    if (ShippingMethodList != null && ShippingMethodList.Count() > 0)
                    {
                        foreach (var item in ShippingMethodList)
                        {
                            item.WarehouseAddress += (Person != null ? $"{Person.PersonCode}(收)" : "");
                        }
                        SelectedShippingMethod = ShippingMethodList.Count() > 0 ? ShippingMethodList.FirstOrDefault() : null;
                    }
                }
                IsBusy = false;
                CurrentState = States.Success;
            });
          
        }

        private ObservableCollection<ProfileMenuItemModel> GetDefaultMainMenuList()
        {
            return new ObservableCollection<ProfileMenuItemModel>()
            {
                 new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Notshipped"],
                    Key= ProfileMenuStatus.Notshipped,
                    Icon= FontAwesome.FontAwesomeIcons.Inventory,
                    Font="fregular"
                },
                  new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Intransit"],
                    Key= ProfileMenuStatus.Intransit,
                    Icon= FontAwesome.FontAwesomeIcons.TruckContainer,
                    Font="fregular"
                },
                   new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Arrivedatwarehouse"],
                    Key= ProfileMenuStatus.Arrivedatwarehouse,
                    Icon= FontAwesome.FontAwesomeIcons.WarehouseAlt,
                    Font="fregular"
                },
                    new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Transactioncompleted"],
                    Key= ProfileMenuStatus.TransactionCompleted,
                    Icon= FontAwesome.FontAwesomeIcons.BoxCheck,
                    Font="fregular"
                }
            };
        }

        private ObservableCollection<ProfileMenuItemModel> GetDefaultSettingsMenuList()
        {
             return new ObservableCollection<ProfileMenuItemModel>()
            {
                 new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Language"],
                    Key=ProfileMenuStatus.Language,
                    Icon= FontAwesome.FontAwesomeIcons.Language,
                    Font="fregular"
                },
                  new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Changepassword"],
                    Key=ProfileMenuStatus.ChangePassword,
                    Icon= FontAwesome.FontAwesomeIcons.LockAlt,
                    Font="fregular"
                },
                   new ProfileMenuItemModel(){
                    Title= this.resourceManager["ls_Personalinfo"],
                    Key=ProfileMenuStatus.PersonalInfo,
                    Icon= FontAwesome.FontAwesomeIcons.UserCog,
                    Font="fregular"
                }
            };
        }
    }
}

