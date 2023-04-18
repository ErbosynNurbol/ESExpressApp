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
    public partial class ArticleViewPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string articleTitle;
        [ObservableProperty]
        private HtmlWebViewSource fullDescription;

        public ArticleViewPageViewModel()
        {
        }

        [RelayCommand]
        void LoadArticle(TypeAndIdModel typeAndId)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                CurrentState = States.Offline;
                return;
            }

            string path = string.Empty;
            switch (typeAndId.Type.Trim().ToLower())
            {
                case "hwork": { path = $"/api/query/Hwork"; } break;
                case "tariff": { path = $"/api/query/Tariff"; } break;
                case "article": { path = $"/api/query/article"; } break;
            }
            var dicParameters = new Dictionary<string, string>
                    {
                      {"id", typeAndId.Id.ToString()}
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query(path, dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                switch (typeAndId.Type.Trim().ToLower())
                {
                    case "hwork":
                        {
                            HWorkModel hwork = JsonHelper.DeserializeObject<HWorkModel>(ajaxMsg.Data.ToString());
                            ArticleTitle = hwork.Title;
                            hwork.FullDescription = ResourceFileHelper.GetHtmlFileContent("ESExpressApp.Resources.Templates.ArticleViewTemplate.html")
                        .Replace("{HtmlContent}", hwork.FullDescription)
                        .Replace("{baseUrl}", APIHelper.baseAddress);
                            FullDescription = new HtmlWebViewSource() { Html = hwork.FullDescription };
                        }
                        break;
                    case "tariff":
                        {
                            TariffModel tariff = JsonHelper.DeserializeObject<TariffModel>(ajaxMsg.Data.ToString());
                            ArticleTitle = tariff.Title;
                            tariff.FullDescription = ResourceFileHelper.GetHtmlFileContent("ESExpressApp.Resources.Templates.ArticleViewTemplate.html")
                        .Replace("{HtmlContent}", tariff.FullDescription)
                        .Replace("{baseUrl}", APIHelper.baseAddress);
                            FullDescription = new HtmlWebViewSource() { Html = tariff.FullDescription };
                        }
                        break;
                    case "article":
                        {
                            ArticleModel article = JsonHelper.DeserializeObject<ArticleModel>(ajaxMsg.Data.ToString());
                            ArticleTitle = article.Title;
                            article.FullDescription = ResourceFileHelper.GetHtmlFileContent("ESExpressApp.Resources.Templates.ArticleViewTemplate.html")
                        .Replace("{HtmlContent}", article.FullDescription)
                        .Replace("{baseUrl}", APIHelper.baseAddress);
                            FullDescription = new HtmlWebViewSource() { Html = article.FullDescription };
                        }
                        break;
                }
                CurrentState = States.Success;
            }
            else
            {
                CurrentState = States.Error;
            }
            IsBusy = false;
        }
    }
}

