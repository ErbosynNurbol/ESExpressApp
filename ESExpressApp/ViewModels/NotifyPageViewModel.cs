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
	public partial class NotifyPageViewModel: BaseViewModel
    {

        [ObservableProperty]
        private ObservableCollection<PushLogModel> pushLogList = new ObservableCollection<PushLogModel>();

        private readonly ILocalizationResourceManager resourceManager;
        public NotifyPageViewModel(ILocalizationResourceManager resourceManager)
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
            LoadPagedData(0);
            IsRefreshing = false;
        }

        [RelayCommand]
        private void LoadMore()
        {
            if (!CanLoadMore || IsLoading) return;
            IsLoading = true;
            int start = PushLogList.Count;
            LoadPagedData(start);
            IsLoading = false;
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
                LoadPagedData(0);
                CurrentState = States.Success;
                IsBusy = false;
            });
        }

        private void LoadPagedData(int start)
        {
            if (start == 0)
            {
                PushLogList.Clear();
            }
            var dicParameters = new Dictionary<string, string>
                    {
                      {"start", start.ToString()},
                      {"length", "10"}
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/pushlist", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                DataPagedModel dataPage = JsonHelper.DeserializeObject<DataPagedModel>(ajaxMsg.Data.ToString());
                if (dataPage != null)
                {
                    var pagedPushLogList = JsonHelper.DeserializeObject<ObservableCollection<PushLogModel>>(dataPage.DataList.ToString());
                    foreach(var item in pagedPushLogList)
                    {
                        PushLogList.Add(item);
                    }
                    CanLoadMore = dataPage.Total > PushLogList.Count();
                }
            }
        }
    }
}

