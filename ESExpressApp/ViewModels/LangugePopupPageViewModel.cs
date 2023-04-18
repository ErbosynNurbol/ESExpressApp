using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ESExpressApp.Models;
using ESExpressApp.Helpers;
using LocalizationResourceManager.Maui;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace ESExpressApp.ViewModels
{
	public partial class LangugePopupPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<LanguageModel> supertedLanuages;

        [ObservableProperty]
        LanguageModel selectedLanguage;

        [ObservableProperty]
        double languageHeight;
        private readonly Views.LangugePopupPage langugePopupPage;
        public LangugePopupPageViewModel(Views.LangugePopupPage langugePopupPage)
        {
            this.langugePopupPage = langugePopupPage;
            LoadData();
        }

        private void LoadData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                CurrentState = States.Offline;
                return;
            }
            if (IsBusy) return;
            IsBusy = true;
            Task.Run(() => {
                AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/languagelist", null);
                if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    SupertedLanuages = JsonHelper.DeserializeObject<ObservableCollection<LanguageModel>>(ajaxMsg.Data.ToString());
                }
                SelectedLanguage = AppSingleton.GetInstance().GetCurrentLanguage();
                if (SelectedLanguage == null)
                {
                    SelectedLanguage = SupertedLanuages.FirstOrDefault(x => x.IsDefault == 1);
                    //AppSingleton.GetInstance().SetCurrentLanguage(SelectedLanguage);
                }
                else
                {
                    SelectedLanguage = SupertedLanuages.FirstOrDefault(x => x.Culture.Equals(SelectedLanguage.Culture));
                }
                IsBusy = false;
                LanguageHeight = SupertedLanuages.Count() * 40;
                CurrentState = States.Success;
            });
        }

        [RelayCommand]
        void ChangeLanguage()
        {
            string key = "appLanguage";
            if (SelectedLanguage == null) return;
            string currentCulture = GetCurrentCulture();
            if (SelectedLanguage.Culture.Equals(currentCulture, StringComparison.OrdinalIgnoreCase))
            {
                this.langugePopupPage.Close();
                return;
            }
            string cultureName = "en";
            switch (SelectedLanguage.Culture.ToLower())
            {
                case "kz": { cultureName = "kk"; } break;
                case "tote": { cultureName = "ar"; } break;
                case "tr": { cultureName = "tr"; } break;
                case "ru": { cultureName = "ru"; } break;
                case "zh-cn": { cultureName = "zh-CN"; } break;
            }
            string appLanguageStr = JsonHelper.SerializeObject(SelectedLanguage);
            Preferences.Set(key, appLanguageStr);
            this.langugePopupPage.resourceManager.CurrentCulture = new CultureInfo(cultureName);
            this.langugePopupPage.Close();
            App.Current.MainPage = new AppShell();
        }

        private string GetCurrentCulture()
        {
            string language = "en";
            switch (this.langugePopupPage.resourceManager.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "kk": { language = "kz"; } break;
                case "ar": { language = "tote"; } break;
                case "tr": { language = "tr"; } break;
                case "ru": { language = "ru"; } break;
                case "zh": { language = "zh-CN"; } break;
            }
            return language;
        }
    }
}

