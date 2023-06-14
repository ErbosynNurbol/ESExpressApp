using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ESExpressApp.Models
{
    public partial class PersonModel : ObservableObject
    {
        [ObservableProperty]
        private int countryId;
        [ObservableProperty]
        private string avatarUrl;
        [ObservableProperty]
        private string personCode;
        [ObservableProperty]
        private string phone;
        [ObservableProperty]
        private string realName;
        [ObservableProperty]
        private string city;
        [ObservableProperty]
        private string whatsApp;
        [ObservableProperty]
        private string secondaryPhone;
    }
}
