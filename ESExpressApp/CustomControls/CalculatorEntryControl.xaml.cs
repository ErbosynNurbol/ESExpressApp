namespace ESExpressApp.CustomControls;

public partial class CalculatorEntryControl : Grid
{
	public CalculatorEntryControl()
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

    public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create(
  propertyName: nameof(ImageUrl),
  returnType: typeof(string),
  declaringType: typeof(CalculatorEntryControl),
  defaultValue: null,
  defaultBindingMode: BindingMode.OneWay);

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set { SetValue(ImageUrlProperty, value); }
    }

    public static readonly BindableProperty CTitleProperty = BindableProperty.Create(
  propertyName: nameof(CTitle),
  returnType: typeof(string),
  declaringType: typeof(CalculatorEntryControl),
  defaultValue: null,
  defaultBindingMode: BindingMode.OneWay);

    public string CTitle
    {
        get => (string)GetValue(CTitleProperty);
        set { SetValue(CTitleProperty, value); }
    }

    public static readonly BindableProperty UnitProperty = BindableProperty.Create(
 propertyName: nameof(Unit),
 returnType: typeof(string),
 declaringType: typeof(CalculatorEntryControl),
 defaultValue: null,
 defaultBindingMode: BindingMode.OneWay);

    public string Unit
    {
        get => (string)GetValue(UnitProperty);
        set { SetValue(UnitProperty, value); }
    }
}
