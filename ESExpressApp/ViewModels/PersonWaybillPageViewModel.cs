using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.ViewModels
{
    public partial class PersonWaybillPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string resultText;

        [ObservableProperty]
        private bool enableEditButton = true;

        [ObservableProperty]
        private PersonIncomingPackageModel personIncomingPackage;

        [ObservableProperty]
        private PackagingTypeModel selectedPackagingType;

        [ObservableProperty]
        private ShippingMethodModel selectedShippingMethod;

        [ObservableProperty]
        private bool visibleShippingMethod = true;

        [ObservableProperty]
        private string addOrEditButtonText;

        private readonly ILocalizationResourceManager resourceManager;
        public PersonWaybillPageViewModel(ILocalizationResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            AddOrEditButtonText = this.resourceManager["ls_Submit"];
        }
        public void LoadPersonWaybill(int personWaybillId)
        {
            var dicParameters = new Dictionary<string, string>
                    {
                      {"id",personWaybillId.ToString()}
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/query/PersonIncomingPackage", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                PersonIncomingPackage = JsonHelper.DeserializeObject<PersonIncomingPackageModel>(ajaxMsg.Data.ToString());
                if (personWaybillId > 0 && PersonIncomingPackage.PersonWaybill.Id == 0)
                {
                    App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], this.resourceManager["ls_Isdeletedoridiswrong"], this.resourceManager["ls_Close"]);
                    GoToAsync("..");
                }
                else
                {
                    if (PersonIncomingPackage.PersonWaybill.Id > 0)
                    {
                        AddOrEditButtonText = this.resourceManager["ls_Save"];
                    }
                    else
                    {
                        AddOrEditButtonText = this.resourceManager["ls_Add"];
                    }
                        
                }
                VisibleShippingMethod = PersonIncomingPackage.PersonWaybill?.ParentId == 0;
              
                _ = Task.Run(async () => {
                    await Task.Delay(100);
                    AppShell.Current.Dispatcher.Dispatch(() => {
                        if (PersonIncomingPackage.PersonWaybill.PackagingTypeId > 0)
                        {
                            SelectedPackagingType = PersonIncomingPackage.PackagingTypeList.FirstOrDefault(p => p.Id == PersonIncomingPackage.PersonWaybill.PackagingTypeId);
                        }
                        else
                        {
                            SelectedPackagingType = null;
                        }
                        OnPropertyChanged(nameof(SelectedPackagingType));
                        if (PersonIncomingPackage.PersonWaybill.ShippingMethodId > 0)
                        {
                            SelectedShippingMethod = PersonIncomingPackage.ShippingMethodList.FirstOrDefault(p => p.Id == PersonIncomingPackage.PersonWaybill.ShippingMethodId);
                        }
                        else
                        {
                            SelectedShippingMethod = PersonIncomingPackage.ShippingMethodList[0];
                        }
                        OnPropertyChanged(nameof(SelectedShippingMethod));
                    });
                });
            }
        }

        [RelayCommand]
        private void AddOrEdit()
        {
            EnableEditButton = false;
            string manageType = PersonIncomingPackage.PersonWaybill.Id > 0 ? "edit" : "create";
            uint packagingTypeId = SelectedPackagingType != null ? SelectedPackagingType.Id : 0;
            uint shippingMethodId = SelectedShippingMethod != null ? SelectedShippingMethod.Id : 0;
            var dicParameters = new Dictionary<string, string>
                    {
                      {"waybillNumber",PersonIncomingPackage.PersonWaybill.WaybillNumber},
                      {"productName",PersonIncomingPackage.PersonWaybill.ProductName},
                      {"clientRemark",PersonIncomingPackage.PersonWaybill.ClientRemark},
                      {"packagingTypeId", packagingTypeId.ToString()},
                      {"shippingMethodId", shippingMethodId.ToString()},
                      {"manageType",manageType},
                      {"id",PersonIncomingPackage.PersonWaybill.Id.ToString()}
                    };
            AjaxMsgModel ajaxMsg = APIHelper.Query($"/api/send/PersonIncomingPackage", dicParameters);
            if (ajaxMsg != null && ajaxMsg.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                if (manageType.Equals("edit"))
                {
                    Toast.Make(ajaxMsg.Message).Show();
                    GoToAsync("..", true);
                }
                else
                {
                    ResultText = ajaxMsg.Message;
                    Toast.Make(ajaxMsg.Message).Show();
                    GoToAsync("..", true);
                }
            }
            else
            {
                string entryName = string.Empty;
                switch (ajaxMsg?.Data)
                {
                    case "waybillNumber":
                        {
                            entryName = $"({this.resourceManager["ls_Waybillnumber"]})";
                        }
                        break;
                    case "productName":
                        {
                            entryName = $"({this.resourceManager["ls_ProductName"]})";
                        }
                        break;
                }

                App.AlertSvc.ShowAlert(this.resourceManager["ls_Warning"], $"{ajaxMsg?.Message}{entryName}", this.resourceManager["ls_Close"]);
                EnableEditButton = true;
            }
        }
    }
}

