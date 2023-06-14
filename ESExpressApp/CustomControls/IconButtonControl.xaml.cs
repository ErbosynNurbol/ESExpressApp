using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Text;

namespace ESExpressApp.CustomControls;

public partial class IconButtonControl : Border
{
	public IconButtonControl()
	{
		InitializeComponent();
	}
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
propertyName: nameof(Icon),
returnType: typeof(string),
declaringType: typeof(IconButtonControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set { SetValue(IconProperty, value); }
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
propertyName: nameof(Title),
returnType: typeof(string),
declaringType: typeof(IconButtonControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set { SetValue(TitleProperty, value); }
    }


    public static readonly BindableProperty TColorProperty = BindableProperty.Create(
propertyName: nameof(TColor),
returnType: typeof(Color),
declaringType: typeof(IconButtonControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public Color TColor
    {
        get => (Color)GetValue(TColorProperty);
        set { SetValue(TColorProperty, value); }
    }

    public static readonly BindableProperty BColorProperty = BindableProperty.Create(
propertyName: nameof(BColor),
returnType: typeof(Color),
declaringType: typeof(IconButtonControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public Color BColor
    {
        get => (Color)GetValue(BColorProperty);
        set { SetValue(BColorProperty, value); }
    }

    public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create(
propertyName: nameof(ClickCommand),
returnType: typeof(IRelayCommand),
declaringType: typeof(IconButtonControl ),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public IRelayCommand ClickCommand
    {
        get => (IRelayCommand)GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly BindableProperty ClickCommandParameterProperty = BindableProperty.Create(
propertyName: nameof(ClickCommandParameter),
returnType: typeof(object),
declaringType: typeof(IconButtonControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public object ClickCommandParameter
    {
        get => (object)GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

}
