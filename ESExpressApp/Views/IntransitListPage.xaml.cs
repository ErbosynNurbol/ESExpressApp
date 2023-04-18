namespace ESExpressApp.Views;

public partial class IntransitListPage : ContentPage
{
    public IntransitListPage(ViewModels.IntransitListPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
