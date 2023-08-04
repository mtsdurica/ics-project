using CommunityToolkit.Mvvm.Input;
using ProjectICS.App.Messages;
using ProjectICS.App.Services;
using ProjectICS.BL.Facades;
using ProjectICS.BL.Models;

namespace ProjectICS.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class UserProfileViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public UserDetailModel User { get; set; } = UserDetailModel.Empty;

    public UserProfileViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _userFacade= userFacade;
        _navigationService= navigationService;
        _alertService=alertService;
    }


    [RelayCommand]
    private async Task SaveAsync()
    {
        if (User.Name == "" || User.Surname =="")
        {
            await _alertService.DisplayAsync("Name Error", "Name or Surname value not inserted");
        }
        else
        {
            await _userFacade.UpdateAsync(User);
            MessengerService.Send(new UserEditMessage { UserId = User.Id });
            _navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task DeleteUserAsync()
    {
        if (User is not null)
        {
            await _userFacade.DeleteAsync(User.Id);
            MessengerService.Send(new UserDeleteMessage());
            _navigationService.SendBackButtonPressed();
            _navigationService.SendBackButtonPressed();
        }
    }


    private async Task ReloadDataAsync()
    {
        User = await _userFacade.GetAsync(User.Id)
               ?? UserDetailModel.Empty;
    }
}