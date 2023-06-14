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
	public partial class HomePageViewModel: BaseViewModel
    {

        [ObservableProperty]
        List<Color> colors;
        [ObservableProperty]
        ObservableCollection<ArticleModel> focusNewsList;
        [ObservableProperty]
        ObservableCollection<TariffModel> tariffList;
        [ObservableProperty]
        ObservableCollection<HWorkModel> hWorkList;
        [ObservableProperty]
        HWorkModel hWorkSelected = null;
        [ObservableProperty]
        ObservableCollection<ClientssayModel> clientsSayList;
        [ObservableProperty]
        double hWorkHeight = 0;
        [ObservableProperty]
        string waybillNumber;

        private readonly ILocalizationResourceManager resourceManager;
        public HomePageViewModel(ILocalizationResourceManager resourceManager)
		{
            this.resourceManager = resourceManager; 
            Colors = new List<Color>()
                            {
                                   Color.FromArgb("#FF5733"),
                                   Color.FromArgb("#333E87"),
                                   Color.FromArgb("#8328B8"),
                            };
            SetDefaultLanguage();
            LoadData();
            //_ = Task.Run(async () => {
            //    await Task.Delay(100);
            //    Dispatcher.Dispatch(() => {
            //        HWorkList = new ObservableCollection<object>(HWorkList);
            //        OnPropertyChanged(nameof(HWorkList));
            //    });
            //});
        }

        [RelayCommand]
        private void Retry()
        {
            SetDefaultLanguage();
            LoadData();
        }

        void SetDefaultLanguage()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }
            LanguageModel currentLanguage = AppSingleton.GetInstance().GetCurrentLanguage();
            if(currentLanguage == null)
            {
                string key = "appLanguage";
                AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/languagelist", null);
                if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                   var supertedLanuages = JsonHelper.DeserializeObject<ObservableCollection<LanguageModel>>(ajaxMsg.Data.ToString());
                    if (supertedLanuages != null)
                    {
                        var selectedLanguage = supertedLanuages.FirstOrDefault(x=>x.IsDefault==1);
                        if(selectedLanguage==null)
                            selectedLanguage = supertedLanuages.FirstOrDefault();

                        string cultureName = "en";
                        switch (selectedLanguage.Culture.ToLower())
                        {
                            case "kz": { cultureName = "kk"; } break;
                            case "tote": { cultureName = "ar"; } break;
                            case "tr": { cultureName = "tr"; } break;
                            case "ru": { cultureName = "ru"; } break;
                            case "zh-cn": { cultureName = "zh-CN"; } break;
                        }
                        string appLanguageStr = JsonHelper.SerializeObject(selectedLanguage);
                        Preferences.Set(key, appLanguageStr);
                        this.resourceManager.CurrentCulture = new CultureInfo(cultureName);
                    }
                }
            }
        }

        [RelayCommand]
        void ShowLanguages()
        {
            Shell.Current.CurrentPage.ShowPopup(new LangugePopupPage(this.resourceManager));
        }

        [RelayCommand]
        void SearchWaybillNumber()
        {
            GoToAsync($"{nameof(WaybillNumberPage)}?waybillNumber={WaybillNumber}");
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
                AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/index", null);
                if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    IndexDataModel indexData = JsonHelper.DeserializeObject<IndexDataModel>(ajaxMsg.Data.ToString());
                    if (indexData?.FocusNewsList != null && indexData?.FocusNewsList.Count() > 0)
                    {
                        foreach (var focusNews in indexData?.FocusNewsList)
                        {
                            focusNews.ThumbnailUrl = focusNews.ThumbnailUrl.Replace("_small.", "_middle.");
                        }
                        FocusNewsList = indexData?.FocusNewsList;
                    }
                    if (indexData?.TariffList != null)
                    {
                        foreach (var tariff in indexData?.TariffList)
                        {
                            tariff.IconUnicode = FontAwesomeHelper.GetFontUnicodeValueByClass(tariff.Icon);
                            tariff.IconFontFamily = FontAwesomeHelper.GetFontFamilyNameByClass(tariff.Icon);
                        }
                        TariffList = indexData?.TariffList;
                    }
                    if (indexData?.HWorkList != null && indexData?.HWorkList.Count() > 0)
                    {
                        foreach (var hWork in indexData?.HWorkList)
                        {
                            hWork.IconUnicode = FontAwesomeHelper.GetFontUnicodeValueByClass(hWork.Icon);
                            hWork.IconFontFamily = FontAwesomeHelper.GetFontFamilyNameByClass(hWork.Icon);
                            hWork.ThumbnailUrl = hWork.ThumbnailUrl.Replace("_small.","_middle.");
                        } 
                        _ = Task.Run(async () => {
                            await Task.Delay(100);
                          AppShell.Current.Dispatcher.Dispatch(() => {
                              HWorkList = indexData?.HWorkList;
                              HWorkHeight = HWorkList.Count() * 67.0;
                              HWorkSelected = HWorkList[0];
                              //this.vm.HWorkList = new System.Collections.ObjectModel.ObservableCollection<HWorkModel>();
                              OnPropertyChanged(nameof(HWorkSelected));
                            });
                        });
                    }

                    ClientsSayList = indexData?.ClientsSayList;
                    CurrentState = States.Success;
                }
                else
                {
                    CurrentState = States.Error;
                }
                IsBusy = false;
            });
        }
    }
}

