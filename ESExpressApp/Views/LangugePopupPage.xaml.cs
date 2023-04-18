using CommunityToolkit.Maui.Views;
using LocalizationResourceManager.Maui;

namespace ESExpressApp.Views;

public partial class LangugePopupPage : Popup
{
    public readonly ILocalizationResourceManager resourceManager;
    public LangugePopupPage(ILocalizationResourceManager resourceManager)
	{
		this.resourceManager = resourceManager;
		InitializeComponent();
		BindingContext = new ViewModels.LangugePopupPageViewModel(this);
    }

    void Close_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        this.Close();
    }
}
