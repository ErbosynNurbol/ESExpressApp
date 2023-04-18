using CommunityToolkit.Mvvm.ComponentModel;
using LocalizationResourceManager.Maui.ComponentModel;
namespace ESExpressApp.ViewModels;

static class States
{
    public const string Loading = nameof(Loading);
    public const string Success = nameof(Success);
    public const string Offline = "Offline";
    public const string Error = "Error";
    public const string Empty = "Empty";
}

public partial class BaseViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    [ObservableProperty]
    bool isRefreshing = false;

    [ObservableProperty]
    bool isLoading = false;

    [ObservableProperty]
    bool isBusy = false;

    [ObservableProperty]
    bool canLoadMore = false;
    
    [ObservableProperty]
    bool canStateChange = false;

    [ObservableProperty]
    string currentState = States.Loading;

    [ObservableProperty]
    private string customStateKey;

    bool isNavigating = false;
    protected Task GoToAsync(string pageWithParam, bool animate = true)
    {
        if (isNavigating)
            return Task.CompletedTask;
        isNavigating = true;
        return Shell.Current.GoToAsync(pageWithParam, animate).ContinueWith((x) => isNavigating = false);
    }

    public virtual Task BackAsync()
    {
        if (isNavigating)
            return Task.CompletedTask;
        isNavigating = true;
        return Shell.Current.GoToAsync("..", true).ContinueWith((x) => isNavigating = false);
    }
}