namespace ESExpressApp.CustomControls;

public partial class CanBackHeaderControl : Grid
{
	public CanBackHeaderControl()
	{
		InitializeComponent();
        LabelBindClick();
    }

    private void LabelBindClick()
    {
        #region Back To Prev
        lblBackToPrev.GestureRecognizers.Add(new TapGestureRecognizer()
        {
            Command = new Command(() =>
            {
                Shell.Current.GoToAsync("..");
            })
        });
        #endregion
    }

    public static readonly BindableProperty BackPageProperty = BindableProperty.Create(
propertyName: nameof(BackPage),
returnType: typeof(string),
declaringType: typeof(CanBackHeaderControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public string BackPage
    {
        get => (string)GetValue(BackPageProperty);
        set { SetValue(BackPageProperty, value); }
    }

    public static readonly BindableProperty PageTitleProperty = BindableProperty.Create(
propertyName: nameof(PageTitle),
returnType: typeof(string),
declaringType: typeof(CanBackHeaderControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public string PageTitle
    {
        get => (string)GetValue(PageTitleProperty);
        set { SetValue(PageTitleProperty, value); }
    }
}
