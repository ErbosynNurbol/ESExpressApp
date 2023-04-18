namespace ESExpressApp.CustomControls;

public partial class PhoneEntryControl : Grid
{
	public PhoneEntryControl()
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

    void CustomEntry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(this.customEntry.Text))
        {
            this.customEntry.Text = "+7";
        }
        this.lblClear.IsVisible = !string.IsNullOrEmpty(e.NewTextValue);
    }

    void Clear_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        this.customEntry.Text = string.Empty;
    }

    void CustomEntry_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(this.customEntry.Text))
        {
            this.customEntry.Text = "+7";
        }
    }
}
