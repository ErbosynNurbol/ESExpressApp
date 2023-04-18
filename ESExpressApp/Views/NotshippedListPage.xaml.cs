namespace ESExpressApp.Views;

public partial class NotshippedListPage : ContentPage
{
	public NotshippedListPage(ViewModels.NotshippedListPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as NotshippedListPageViewModel).RefreshCommand.Execute(null);
    }
}
