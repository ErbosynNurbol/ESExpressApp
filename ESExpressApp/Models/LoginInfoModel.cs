using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ESExpressApp.Models
{
    public partial class LoginInfoModel : ObservableObject
    {
        [ObservableProperty]
        private PersonModel userInfo;
        public string QarToken { get; set; }
        public int UID { get; set; }
    }
}