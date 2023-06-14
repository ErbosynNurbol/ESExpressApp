using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using System.Windows.Input;

namespace ESExpressApp.ViewModels
{
	public partial class NotshippedListPageViewModel : BaseViewModel
    {

        [ObservableProperty]
        ObservableCollection<PackageInfoModel> personWaybillList = new ObservableCollection<PackageInfoModel>();

        private readonly ILocalizationResourceManager resourceManager;
        public NotshippedListPageViewModel(ILocalizationResourceManager resourceManager)
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
        }

        [RelayCommand]
        private void Delete(object obj)
        {
            PackageInfoModel packageInfo = obj as PackageInfoModel;
            if (packageInfo != null)
            {
                App.AlertSvc.ShowConfirmation(this.resourceManager["ls_Delete"], this.resourceManager["ls_Areyousure"], (result =>
                {
                    if (result)
                    {
                        var dicParameters = new Dictionary<string, string>
                        {
                            {"manageType","delete"},
                            {"id",packageInfo.Id.ToString()}
                        };
                        AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/send/PersonIncomingPackage", dicParameters);
                        if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                        {
                            PersonWaybillList.Remove(packageInfo);
                            Toast.Make(ajaxMsg.Message).Show();
                        }
                        else
                        {
                            App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}", this.resourceManager["ls_Close"]);
                        }
                    }
                   
                }), this.resourceManager["ls_Delete"], this.resourceManager["ls_Close"]);
            }
        }

        [RelayCommand]
        private void AddNew()
        {
            GoToAsync($"{nameof(PersonWaybillPage)}", true);
        }

        [RelayCommand]
        private void Edit(object obj)
        {
            PackageInfoModel packageInfo = obj as PackageInfoModel;
            if (packageInfo != null)
            {
               GoToAsync($"{nameof(PersonWaybillPage)}?itemId={packageInfo.Id}", true);
            }
        }
        public void LoadPersonWaybillList(int start)
        {
            if (IsBusy) return;
            IsBusy = true;
            var dicParameters = new Dictionary<string, string>
                    {
                      {"manageType",ProfileMenuStatus.Notshipped.ToString().ToLower()},
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

