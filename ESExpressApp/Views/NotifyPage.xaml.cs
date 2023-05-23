namespace ESExpressApp.Views;

public partial class NotifyPage : ContentPage
{
    public NotifyPage(ViewModels.NotifyPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
