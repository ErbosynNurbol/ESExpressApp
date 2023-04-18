namespace ESExpressApp.Views;


[QueryProperty("ItemType", "itemType")]
[QueryProperty("ItemId", "itemId")]
[QueryProperty("ItemTitle", "itemTitle")]
public partial class ArticleViewPage : ContentPage
{
    private bool isAppearing = false;
    public ArticleViewPage()
	{
		InitializeComponent();
        BindingContext = new ViewModels.ArticleViewPageViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isAppearing)
        {
            var viewModel = (BindingContext as ViewModels.ArticleViewPageViewModel);
            if (viewModel.LoadArticleCommand.CanExecute(null))
            {
                viewModel.LoadArticleCommand.Execute(new Models.TypeAndIdModel { Type = ItemType, Id = ItemId });
            }
            isAppearing = true;
        }
    }

    public string ItemType { get; set; }
    public int ItemId { get; set; }
    public string ItemTitle { get; set; }
}
