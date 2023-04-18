namespace ESExpressApp.Views;

[QueryProperty("WaybillNumber", "waybillNumber")]
public partial class WaybillNumberPage : ContentPage
{

    private bool isAppearing = false;
    public WaybillNumberPage(WaybillNumberPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isAppearing)
        {
            isAppearing = true;
            if (!string.IsNullOrEmpty(WaybillNumber) && !string.IsNullOrEmpty(WaybillNumber = WaybillNumber.Trim()))
            {
                //entrySearch.Text = WaybillNumber;
                (BindingContext as WaybillNumberPageViewModel).WaybillNumber = WaybillNumber;
                (BindingContext as WaybillNumberPageViewModel).Search();
            }
        }

    }
    public string WaybillNumber { get; set; }
}
