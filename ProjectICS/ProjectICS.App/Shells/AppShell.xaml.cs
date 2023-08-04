using CommunityToolkit.Mvvm.Input;
using ProjectICS.App.Services;
using ProjectICS.App.Messages;
using ProjectICS.App.PopUps;
using ProjectICS.App.Views;
using CommunityToolkit.Maui.Views;

namespace ProjectICS.App.Shells;

public partial class AppShell
{
    private readonly IMessengerService _messengerService;
    public AppShell(IMessengerService messengerService)
    {
        _messengerService = messengerService;

        InitializeComponent();
        
    }

    [RelayCommand]
    private void Refresh()
        => _messengerService.Send(new RefreshMessage());

    [RelayCommand]
    private void ExitApplication()
        => Environment.Exit(0);

    [RelayCommand]
    private void ShowAboutPopUp()
        => this.ShowPopup(new PopUpAboutPage());

    [RelayCommand]
    private void ShowHelpPopUp()
        => this.ShowPopup(new PopUpHelpPage());

}