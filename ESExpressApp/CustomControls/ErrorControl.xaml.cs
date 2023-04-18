using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace ESExpressApp.CustomControls;

public partial class ErrorControl : Grid
{
	public ErrorControl()
	{
		InitializeComponent();
	}
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
propertyName: nameof(Icon),
returnType: typeof(string),
declaringType: typeof(ErrorControl),
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
declaringType: typeof(ErrorControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set { SetValue(TitleProperty, value); }
    }

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
propertyName: nameof(Description),
returnType: typeof(string),
declaringType: typeof(ErrorControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set { SetValue(DescriptionProperty, value); }
    }

    public static readonly BindableProperty RetryCommandProperty = BindableProperty.Create(
propertyName: nameof(RetryCommand),
returnType: typeof(IRelayCommand),
declaringType: typeof(ErrorControl),
defaultValue: null,
defaultBindingMode: BindingMode.OneWay);

    public IRelayCommand RetryCommand
    {
        get => (IRelayCommand)GetValue(RetryCommandProperty);
        set => SetValue(RetryCommandProperty, value);
    }

}
