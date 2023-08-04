using CommunityToolkit.Mvvm.Input;
using ProjectICS.App.Messages;
using ProjectICS.App.Services;
using ProjectICS.BL.Facades;
using ProjectICS.BL.Models;

namespace ProjectICS.App.ViewModels;
[QueryProperty(nameof(User), nameof(User))]
public partial class UserEditViewModel : ViewModelBase
{

	private readonly IUserFacade _userFacade;
	private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

	public UserDetailModel User { get; set; } = UserDetailModel.Empty;
	public UserEditViewModel(IUserFacade userFacade, 
        INavigationService navigationService, 
        IMessengerService messengerService,
        IAlertService alertService) 
        : base(messengerService)
	{
		_userFacade = userFacade;
		_navigationService = navigationService;
        _alertService = alertService;
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
            await _userFacade.SaveAsync(User);
            MessengerService.Send(new UserEditMessage { UserId = User.Id });
            _navigationService.SendBackButtonPressed();
        }
    }
}