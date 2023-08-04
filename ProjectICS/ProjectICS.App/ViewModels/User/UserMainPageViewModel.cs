using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProjectICS.App.Messages;
using ProjectICS.App.Services;
using ProjectICS.App.ViewModels;
using ProjectICS.BL.Facades;
using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectICS.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserMainPageViewModel : ViewModelBase, IRecipient<UserEditMessage>, IRecipient<ActivityEditMessage>, 
                                            IRecipient<ProjectCreateMessage>, IRecipient<ProjectEditMessage>, 
                                            IRecipient<ProjectDeleteMessage>, IRecipient<ActivityDeleteMessage>
{
    private readonly IUserFacade _userFacade;
    private readonly IProjectFacade _projectFacade;
    private readonly IActivityFacade _activityFacade;
    private readonly IProjectModelMapper _projectModelMapper;
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;
    private readonly IAlertService _alertService;

    public Guid Id { get; set; }
    public UserDetailModel User { get; set; } = UserDetailModel.Empty;

    public ObservableCollection<ActivityListModel> UserActivities { get; set; }

    public List<string> firstFilteringOption { get; } = new List<string> { "This day", "This week", "Last week", "This month", "Last month", "This year" };
    public List<string> secondFilteringOption { get; } = new List<string> { "Ascending", "Descending" };

    public string? SelectedFirstFilteringOption { get; set; }

    public string? SelectedSecondFilteringOption { get; set; }

    public UserMainPageViewModel(
        IUserFacade userFacade,
        IProjectFacade projectFacade,
        IActivityFacade activityFacade,
        IProjectModelMapper projectModelMapper,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _projectFacade = projectFacade;
        _projectModelMapper = projectModelMapper;
        _messengerService = messengerService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task GoToEditUserAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/profile",
                new Dictionary<string, object?> { [nameof(UserProfileViewModel.User)] = User with { } });
        }
    }

    [RelayCommand]
    private async Task GoToCreateActivityAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/createActivity",
                new Dictionary<string, object?> { [nameof(ActivityCreateViewModel.User)] = User with { } });
        }
        
    }

    [RelayCommand]
    private async Task GoToCreateProjectAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/createProject",
                new Dictionary<string, object?> { [nameof(ProjectCreateViewModel.User)] = User with { } });
        }
    }

    [RelayCommand]
    private void LogOut()
    {
        MessengerService.Send(new RefreshMessage());
        _navigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    private async Task GoToProject(Guid Id)
        => await _navigationService.GoToAsync<ProjectDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ProjectDetailViewModel.Id)] = Id, 
                [nameof(ProjectDetailViewModel.User)] = User with { } });
            

    [RelayCommand]
    private async Task GoToActivity(Guid Id)
        => await _navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = Id });


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        User = await _userFacade.GetAsync(Id);

        foreach (var userActivity in User.UserActivities)
        {
            if (userActivity.ProjectId is not null)
            {
                var tmp = await _projectFacade.GetAsync((Guid)userActivity.ProjectId);
                if (tmp is not null)
                {
                    if (User.UserProjects.Contains(_projectModelMapper.MapToListModel(tmp)) is not true)
                    {
                        User.UserProjects.Add(_projectModelMapper.MapToListModel(tmp));
                    }
                }
            }
        }

        if (SelectedSecondFilteringOption is not null)
        {
            IEnumerable<ActivityListModel> filteredActivities = await _activityFacade.GetFilteredActivitiesAsync(User.Id, SelectedFirstFilteringOption, SelectedSecondFilteringOption);
            UserActivities = filteredActivities.ToObservableCollection();
        }
        else if(SelectedSecondFilteringOption is null && SelectedFirstFilteringOption is not null)
        {
            await _alertService.DisplayAsync("Filter warning","Order can not be unspecified!");
        }
        else if (SelectedSecondFilteringOption is null)
        {
            UserActivities = User.UserActivities;
        }

        SelectedFirstFilteringOption = null;
        SelectedSecondFilteringOption = null;
    }


    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectCreateMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectEditMessage message)
    {
        await LoadDataAsync();
    }
}