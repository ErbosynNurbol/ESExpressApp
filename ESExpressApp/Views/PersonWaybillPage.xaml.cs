namespace ESExpressApp.Views;

[QueryProperty("ItemId", "itemId")]
public partial class PersonWaybillPage : ContentPage
{
    public PersonWaybillPage(ViewModels.PersonWaybillPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as PersonWaybillPageViewModel).LoadPersonWaybill(ItemId);
    }
  
    public int ItemId { get; set; }
}
