using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProjectICS.App.Services;
using ProjectICS.App.Messages;
using ProjectICS.BL.Facades;
using ProjectICS.BL.Models;

namespace ProjectICS.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityDetailViewModel : ViewModelBase, IRecipient<ActivityEditMessage>
{
    private readonly IActivityFacade _activityFacade;
    private readonly IProjectFacade _projectFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public Guid Id { get; set; }
    public ActivityDetailModel? Activity { get; set; }
    public ProjectDetailModel Project {get; set; } = ProjectDetailModel.Empty;

    public TimeSpan StartTempTime { get; set; } = TimeSpan.Zero;
    public TimeSpan EndTempTime { get; set; } = TimeSpan.Zero;
    public TimeSpan ActivityDuration { get; set; } = TimeSpan.Zero;
    public DateTime Date { get; set; } = DateTime.MinValue;

    public ActivityDetailViewModel(
        IActivityFacade activityFacade, 
        IProjectFacade projectFacade,
        INavigationService navigationService, 
        IMessengerService messengerService,
        IAlertService alertService)
            : base(messengerService)
    {
        _activityFacade = activityFacade;
        _projectFacade = projectFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity is not null)
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
                        Date.Year,
                        Date.Month,
                        Date.Day,
                        StartTempTime.Hours,
                        StartTempTime.Minutes,
                        StartTempTime.Seconds
                    );
                    Activity.EndTime = new DateTime(
                        Date.Year,
                        Date.Month,
                        Date.Day,
                        EndTempTime.Hours,
                        EndTempTime.Minutes,
                        EndTempTime.Seconds
                    );
                    await _activityFacade.UpdateAsync(Activity);
                    MessengerService.Send(new ActivityEditMessage());
                }
                catch (InvalidOperationException)
                {
                    await _alertService.DisplayAsync("Datetime Error ",
                        "Inserted value in time or date is either colliding with another activity or end of activity is before start of activity");
                }
            }
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Activity is not null)
        {
            await _activityFacade.DeleteAsync(Activity.Id);
            MessengerService.Send(new ActivityDeleteMessage{ UserId = Activity.UserId });
            _navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task DeleteFromProjAsync()
    {
        Activity.ProjectId = null;
        await _activityFacade.UpdateAsync(Activity);
        MessengerService.Send(new ActivityEditMessage());
    }
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activity = await _activityFacade.GetAsync(Id);
        if (Activity.ProjectId is not null)
        {
            Project = await _projectFacade.GetProjByIdAsync((Guid)Activity.ProjectId);
        }
        else
        {
            Project.Name = "No project";
        }

        if (Activity is not null)
        {
            StartTempTime = new(Activity.StartTime.Hour, Activity.StartTime.Minute, Activity.StartTime.Second);
            EndTempTime = new(Activity.EndTime.Hour, Activity.EndTime.Minute, Activity.EndTime.Second);
            Date = new DateTime(Activity.StartTime.Year, Activity.StartTime.Month, Activity.StartTime.Day);
        }

        ActivityDuration = EndTempTime - StartTempTime;
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }
}
