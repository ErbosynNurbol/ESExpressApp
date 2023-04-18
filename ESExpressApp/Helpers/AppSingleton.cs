using ESExpressApp.Models;
using LocalizationResourceManager.Maui;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ESExpressApp.Helpers
{
    public class AppSingleton
    {
        private static AppSingleton instance;
        private static object _lock = new object();
        private static object _lockLanguage = new object();
        private ConcurrentDictionary<string, LoginInfoModel> loginInfoDictionary = new ConcurrentDictionary<string, LoginInfoModel>();
        private ConcurrentDictionary<string, LanguageModel> languageDictionary = new ConcurrentDictionary<string, LanguageModel>();
        private AppSingleton() { }

        public static AppSingleton GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new AppSingleton();
                    }
                }
            }
            return instance;
        }

        #region Language
        public  LanguageModel GetCurrentLanguage()
        {
            string key = "appLanguage";
            if (languageDictionary.ContainsKey(key))
            {
                return languageDictionary[key];
            }
            else
            {
                string appLanguageStr = Preferences.Get("appLanguage", null);
                LanguageModel appLanguage = null;
                if (!string.IsNullOrEmpty(appLanguageStr))
                {
                    try
                    {
                        appLanguage = JsonHelper.DeserializeObject<LanguageModel>(appLanguageStr);
                        //SetCurrentLanguage(appLanguage);
                    }catch 
                    {
                        return null;
                    }
                }
                return appLanguage;
            }
        }
        private string GetCurrentCulture(ILocalizationResourceManager resourceManager)
        {
            string language = "en";
            if (resourceManager != null)
            {
                switch (resourceManager.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "kk": { language = "kz"; } break;
                    case "ar": { language = "tote"; } break;
                    case "tr": { language = "tr"; } break;
                    case "ru": { language = "ru"; } break;
                    case "zh": { language = "zh-CN"; } break;
                }
            }
            return language;
        }
        //public  void SetCurrentLanguage(LanguageModel appLanguage)
        //{
        //    string key = "appLanguage";
        //    lock (_lockLanguage)
        //    {
        //        if (appLanguage == null) return;
        //        string currentCulture = GetCurrentCulture();
        //        if (appLanguage.Culture.Equals(currentCulture, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return;
        //        }
        //        string cultureName = "en";
        //        switch (appLanguage.Culture.ToLower())
        //        {
        //            case "kz": { cultureName = "kk"; } break;
        //            case "tote": { cultureName = "ar"; } break;
        //            case "tr": { cultureName = "tr"; } break;
        //            case "ru": { cultureName = "ru"; } break;
        //            case "zh-cn": { cultureName = "zh-CN"; } break;
        //        }
        //        languageDictionary.AddOrUpdate(key, appLanguage, (oldKey, oldValue) => appLanguage);
        //        string appLanguageStr = JsonHelper.SerializeObject(appLanguage);
        //        Preferences.Set(key, appLanguageStr);
        //        LocalizationResourceManager.Current.CurrentCulture = new CultureInfo(cultureName);
        //        // Sets the required culture to the static texts in the control.
        //        //if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
        //        //{
        //        //    DependencyService.Get<Helpers.ILocalize>().SetLocale(new CultureInfo(cultureName));
        //        //}

        //        //App.Current.MainPage = new AppShell();
        //    }
        //}
        #endregion

        #region Login Info
        public LoginInfoModel GetLoginInfo()
        {
            string key = "loginInfo";
            if (loginInfoDictionary.ContainsKey(key))
            {
                return loginInfoDictionary[key];
            }
            return null;

        }
        public void SetLoginInfo(LoginInfoModel loginInfo)
        {
            string key = "loginInfo";
            loginInfoDictionary.AddOrUpdate(key, loginInfo, (oldKey, oldValue) => loginInfo);
        }
        #endregion

    }
}
