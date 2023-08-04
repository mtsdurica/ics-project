using CommunityToolkit.Mvvm.Input;
using ProjectICS.App.Messages;
using ProjectICS.App.Services;
using ProjectICS.BL.Facades;
using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;

namespace ProjectICS.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class ActivityCreateViewModel : ViewModelBase
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IActivityModelMapper _activityModelMapper;
    private readonly IAlertService _alertService;

    public UserDetailModel? User { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public TimeSpan StartTempTime { get; init; } = TimeSpan.Zero;
    public TimeSpan EndTempTime { get; init; } = TimeSpan.Zero;

    public ActivityCreateViewModel(
        IActivityFacade activityFacade,
        IActivityModelMapper activityModelMapper,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _activityModelMapper = activityModelMapper;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity is not null && User is not null)
        {
            if (Activity.Name == "")
            {
                await _alertService.DisplayAsync("Name Error", "Name value not inserted");
            }
            else
            {
                try
                {
                    Activity.StartTime = new DateTime(
                        Activity.StartTime.Year,
                        Activity.StartTime.Month,
                        Activity.StartTime.Day,
                        StartTempTime.Hours,
                        StartTempTime.Minutes,
                        StartTempTime.Seconds
                    );
                    Activity.EndTime = new DateTime(
                        Activity.StartTime.Year,
                        Activity.StartTime.Month,
                        Activity.StartTime.Day,
                        EndTempTime.Hours,
                        EndTempTime.Minutes,
                        EndTempTime.Seconds
                    );
                    await _activityFacade.SaveAsync(Activity, User.Id);
                    User.UserActivities.Add(_activityModelMapper.MapToListModel(Activity));
                    MessengerService.Send(new ActivityEditMessage ());
                    _navigationService.SendBackButtonPressed();
                }
                catch (InvalidOperationException)
                {
                    await _alertService.DisplayAsync("Datetime Error ",
                        "Inserted value in time or date is either colliding with another activitiy or end of activity is before start of activity");
                }
            }
        }
    }


}