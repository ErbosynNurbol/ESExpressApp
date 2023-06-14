using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ESExpressApp.Models
{
	public partial class ProfileMenuItemModel:ObservableObject
	{
        [ObservableProperty]
        public string title;
        [ObservableProperty]
        public ProfileMenuStatus key;
        [ObservableProperty]
        public string icon;
        [ObservableProperty]
        public string font;
        [ObservableProperty]
        public int count;
        [ObservableProperty]
        public bool showCount;
    }
}

