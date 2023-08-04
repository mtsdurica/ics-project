using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ProjectICS.App.Messages;
using ProjectICS.App.Services;

namespace ProjectICS.App.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, IViewModel, IRecipient<RefreshMessage>
{
    private bool _isRefreshRequired = true;

    protected readonly IMessengerService MessengerService;

    protected ViewModelBase(IMessengerService messengerService)
        : base(messengerService.Messenger)
    {
        MessengerService = messengerService;
        IsActive = true;
    }

    public async Task OnAppearingAsync()
    {
        if (_isRefreshRequired)
        {
            await LoadDataAsync();

            _isRefreshRequired = false;
        }
    }

    protected virtual Task LoadDataAsync()
        => Task.CompletedTask;

    public async void Receive(RefreshMessage message)
    {
        await LoadDataAsync();
    }

}
