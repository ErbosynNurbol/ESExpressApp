using CommunityToolkit.Mvvm.Input;

namespace ESExpressApp.CustomControls;

public partial class SearchWaybillNumberControl : Grid
{
	public SearchWaybillNumberControl()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
      propertyName: nameof(Text),
      returnType: typeof(string),
      declaringType: typeof(CalculatorEntryControl),
      defaultValue: null,
      defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
    propertyName: nameof(Placeholder),
    returnType: typeof(string),
    declaringType: typeof(CalculatorEntryControl),
    defaultValue: null,
    defaultBindingMode: BindingMode.OneWay);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set { SetValue(PlaceholderProperty, value); }
    }

    public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(
propertyName: nameof(SearchCommand),
returnType: typeof(IRelayCommand),
declaringType: typeof(ErrorControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public IRelayCommand SearchCommand
    {
        get => (IRelayCommand)GetValue(SearchCommandProperty);
        set => SetValue(SearchCommandProperty, value);
    }
}
