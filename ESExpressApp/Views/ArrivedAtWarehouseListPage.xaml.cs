namespace ESExpressApp.Views;

public partial class ArrivedAtWarehouseListPage : ContentPage
{
    public ArrivedAtWarehouseListPage(ViewModels.ArrivedAtWarehouseListPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
