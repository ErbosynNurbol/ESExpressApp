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
	public partial class InfoPageViewModel : BaseViewModel
    {

        [ObservableProperty]
        ObservableCollection<ArticleModel> articleList;
        [ObservableProperty]
        ArticleModel articleSelected;

        public InfoPageViewModel()
		{
            LoadData();
        }

        [RelayCommand]
        private void Retry()
        {
            LoadData();
        }

        [RelayCommand]
        void ArticleSelection()
        {
            if (ArticleSelected != null)
            {
                Shell.Current.GoToAsync($"{nameof(ArticleViewPage)}?itemType=article&itemId={ArticleSelected.Id}", true);
                ArticleSelected = null;
            }
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
                AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/articlelist", null);
                if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    ArticleList = JsonHelper.DeserializeObject<ObservableCollection<ArticleModel>>(ajaxMsg.Data.ToString());
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

