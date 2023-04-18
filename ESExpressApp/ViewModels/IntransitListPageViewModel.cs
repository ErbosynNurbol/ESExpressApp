using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.ViewModels
{
	public partial class IntransitListPageViewModel : BaseViewModel
    {

        [ObservableProperty]
        ObservableCollection<PackageInfoModel> personWaybillList = new ObservableCollection<PackageInfoModel>();

        private readonly ILocalizationResourceManager resourceManager;
        public IntransitListPageViewModel(ILocalizationResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            LoadData();
        }
        [RelayCommand]
        private void Retry()
        {
            LoadData();
        }

        [RelayCommand]
        private void Refresh()
        {
            IsRefreshing = true;
            LoadPersonWaybillList(0);
            IsRefreshing = false;
        }

        [RelayCommand]
        private void LoadMore()
        {
            if (!CanLoadMore|| IsLoading) return ;
            IsLoading = true;
            int start = PersonWaybillList.Count;
            LoadPersonWaybillList(start);
            IsLoading = false;
        }

        [RelayCommand]
        private void Search(object obj)
        {
            PackageInfoModel packageInfo = obj as PackageInfoModel;
            if (packageInfo != null)
            {
                GoToAsync($"{nameof(WaybillNumberPage)}?waybillNumber={packageInfo.WaybillNumber}");
            }
        }

        public void LoadPersonWaybillList(int start)
        {
            if (IsBusy) return;
            IsBusy = true;

            var dicParameters = new Dictionary<string, string>
                    {
                      {"manageType",ProfileMenuStatus.Intransit.ToString().ToLower()},
                      {"start",start.ToString()},
                      {"length","15"}
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/PersonPackageList", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                DataPagedModel dataPage = JsonHelper.DeserializeObject<DataPagedModel>(ajaxMsg.Data.ToString());
                if (dataPage != null)
                {
                    var pagedPersonWaybillList = JsonHelper.DeserializeObject<ObservableCollection<PackageInfoModel>>(dataPage.DataList.ToString());
                    if (start == 0)
                    {
                        PersonWaybillList.Clear();
                    }
                    foreach (var item in pagedPersonWaybillList)
                    {
                        PersonWaybillList.Add(item);
                    }
                    CanLoadMore = dataPage.Total > PersonWaybillList.Count;
                }
                CurrentState = States.Success;
            }
            else
            {
                CurrentState = States.Error;
            }
            IsBusy = false;
        }

        private void LoadData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                CurrentState = States.Offline;
                return;
            }
            LoadPersonWaybillList(0);
        }
    }
}

