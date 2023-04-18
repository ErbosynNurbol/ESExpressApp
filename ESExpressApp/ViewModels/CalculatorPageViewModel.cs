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
	public partial class CalculatorPageViewModel : BaseViewModel
    {

        [ObservableProperty]
        ObservableCollection<CalculatorTypeModel> calculatorTypeList;
        [ObservableProperty]
        CalculatorTypeModel selectedCalculatorType;
        private readonly ILocalizationResourceManager resourceManager;
        public CalculatorPageViewModel(ILocalizationResourceManager resourceManager)
		{
            this.resourceManager = resourceManager;
            LoadData();
        }

        [RelayCommand]
        private void Retry()
        {
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
            CalculatorTypeList = new ObservableCollection<CalculatorTypeModel>()
            {
                new CalculatorTypeModel(){
                    Title = this.resourceManager["ls_Airtransportation"],
                    Type = "air",
                    IconImage ="air.png"
                },
                 new CalculatorTypeModel(){
                    Title = this.resourceManager["ls_Trucktransportation"],
                    Type = "truct",
                    IconImage ="truct.png"
                },
                  new CalculatorTypeModel(){
                    Title = this.resourceManager["ls_Traintransportation"],
                    Type = "train",
                    IconImage ="train"
                }
            };
            SelectedCalculatorType = CalculatorTypeList.FirstOrDefault();
            CurrentState = States.Success;
            IsBusy = false;
        }
    }
}

