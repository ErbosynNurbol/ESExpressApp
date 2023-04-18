using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.ViewModels
{
    public partial class WaybillNumberPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        string waybillNumber;
        [ObservableProperty]
        ObservableCollection<WaybillInfoModel> waybillInfoList = new ObservableCollection<WaybillInfoModel>();
        private readonly ILocalizationResourceManager resourceManager;
        public WaybillNumberPageViewModel(ILocalizationResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            Search();
        }

        [RelayCommand]
        private void Retry()
        {
            CurrentState = States.Loading;
            Search();
        }

        [RelayCommand]
        public void Refresh()
        {
            IsRefreshing = true;
            Search();
            IsRefreshing = false;
        }

        [RelayCommand]
        public void Search()
        {
            if (string.IsNullOrEmpty(WaybillNumber) || string.IsNullOrEmpty(WaybillNumber.Trim()))
                return;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                CurrentState = States.Offline;
                return;
            }
            if (IsBusy) return;
            IsBusy = true;
            WaybillInfoList.Clear();
            var dicParameters = new Dictionary<string, string>
                    {
                      {"keyWord",WaybillNumber}
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/waybill", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                var searchedWaybillInfoList = new ObservableCollection<WaybillInfoModel>();
                List<WaybillInfoModel> dataList = JsonHelper.DeserializeObject<List<WaybillInfoModel>>(ajaxMsg.Data.ToString());
                if (dataList.Count > 0)
                {
                    foreach (WaybillInfoModel data in dataList)
                    {
                        switch (data.Status)
                        {
                            case 0:
                                {
                                    searchedWaybillInfoList.Add(new WaybillInfoModel()
                                    {
                                        Address = data.Address,
                                        Date = data.Date,
                                        Time = data.Time,
                                        Week = data.Week,
                                        Status = data.Status,
                                        Remark = data.Remark,
                                        Icon = FontAwesome.FontAwesomeIcons.CheckCircle,
                                        Font = "fregular",
                                        FontSize = 12,
                                        TextColor = "#ffffff",
                                        ShowLine = true
                                    });
                                }
                                break;
                            case 2:
                                {
                                    searchedWaybillInfoList.Add(new WaybillInfoModel()
                                    {
                                        Address = data.Address,
                                        Date = data.Date,
                                        Time = data.Time,
                                        Week = data.Week,
                                        Status = data.Status,
                                        Remark = data.Remark,
                                        Icon = FontAwesome.FontAwesomeIcons.ChevronCircleDown,
                                        Font = "fregular",
                                        FontSize = 12,
                                        TextColor = "#ffffff", //"#929292",
                                        ShowLine = true
                                    });

                                }
                                break;
                            case 5:
                                {
                                    searchedWaybillInfoList.Add(new WaybillInfoModel()
                                    {
                                        Address = data.Address,
                                        Date = data.Date,
                                        Time = data.Time,
                                        Week = data.Week,
                                        Status = data.Status,
                                        Remark = data.Remark,
                                        Icon = FontAwesome.FontAwesomeIcons.CheckCircle,
                                        Font = "fregular",
                                        FontSize = 12,
                                        TextColor = string.IsNullOrEmpty(data.Date) ? "#929292" : "#ffffff",
                                        ShowLine = false
                                    });

                                }
                                break;
                        }
                    }
                    foreach(var item in searchedWaybillInfoList)
                    {
                        WaybillInfoList.Add(item);
                    }
                    CurrentState = States.Success;
                }
                else
                {
                    CurrentState = States.Empty;
                }
            }
            else
            {
                CurrentState = States.Error;
            }
            IsBusy = false;
        }
    }
}

