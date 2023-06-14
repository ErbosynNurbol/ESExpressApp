namespace ESExpressApp.Views;

public partial class NotshippedListPage : ContentPage
{
    ViewModels.NotshippedListPageViewModel vm;
    public NotshippedListPage(ViewModels.NotshippedListPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = this.vm = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.vm.RefreshCommand.Execute(null);
    }
}
