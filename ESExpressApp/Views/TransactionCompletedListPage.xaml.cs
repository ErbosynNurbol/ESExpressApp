namespace ESExpressApp.Views;

public partial class TransactionCompletedListPage : ContentPage
{
    public TransactionCompletedListPage(ViewModels.TransactionCompletedListPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
