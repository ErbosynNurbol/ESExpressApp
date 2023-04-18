using FontAwesome;

namespace ESExpressApp.CustomControls;

public partial class PasswordEntryControl : Grid
{
	public PasswordEntryControl()
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

    void ShowOrHidden_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        this.lblShowOrHidden.Text = this.customEntry.IsPassword ? FontAwesomeIcons.EyeSlash : FontAwesomeIcons.Eye;
        this.customEntry.IsPassword = !this.customEntry.IsPassword;
    }
}
