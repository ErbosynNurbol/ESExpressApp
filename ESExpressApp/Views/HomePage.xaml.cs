using ESExpressApp.Models;

namespace ESExpressApp.Views;

public partial class HomePage : ContentPage
{
	public HomePage(ViewModels.HomePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
    void OnPreviousButtonClicked(System.Object sender, System.EventArgs e)
    {
    }

    void OnNextButtonClicked(System.Object sender, System.EventArgs e)
    {
    }

    void Tariff_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        TariffModel tariff = ((TappedEventArgs)e).Parameter as TariffModel;
        if (tariff != null)
        {
            Shell.Current.GoToAsync($"{nameof(ArticleViewPage)}?itemType=tariff&itemId={tariff.Id}", true);
        }
    }
}
