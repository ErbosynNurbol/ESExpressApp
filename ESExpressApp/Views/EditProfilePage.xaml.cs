
namespace ESExpressApp.Views;

public partial class EditProfilePage : ContentPage
{

    private bool isAppearing = false;
    public EditProfilePage(EditProfilePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (isAppearing)
        {
            (BindingContext as EditProfilePageViewModel).Refresh();
        }
    }
}
