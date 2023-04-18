using LocalizationResourceManager.Maui;

namespace ESExpressApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ViewModels.ProfilePageViewModel vm)
    {
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ProfilePageViewModel).Refresh();
    }
}
