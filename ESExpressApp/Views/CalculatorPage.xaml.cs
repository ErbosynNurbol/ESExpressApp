namespace ESExpressApp.Views;

public partial class CalculatorPage : ContentPage
{
	public CalculatorPage(ViewModels.CalculatorPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
