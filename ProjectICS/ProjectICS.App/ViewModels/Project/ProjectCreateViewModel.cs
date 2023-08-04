using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ProjectICS.App.Messages;
using ProjectICS.App.Services;
using ProjectICS.BL.Facades;
using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;

namespace ProjectICS.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]

public partial class ProjectCreateViewModel : ViewModelBase
{
    private readonly IProjectFacade _projectFacade;
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IProjectModelMapper _projectModelMapper;
    private readonly IAlertService _alertService;

    public UserDetailModel? User { get; set; }
    public ProjectDetailModel Project { get; set; } = ProjectDetailModel.Empty;
    public ActivityListModel? SelectedActivity { get; set; }
    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
    public ProjectListModel? SelectedProjectListModel { get; set; }
    public ObservableCollection<ProjectListModel> Projects { get; set; } = new();

    public ProjectCreateViewModel(
        IProjectFacade projectFacade,
        IActivityFacade activityFacade,
        IProjectModelMapper projectModelMapper,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _projectFacade = projectFacade;
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _projectModelMapper = projectModelMapper;
        _alertService=alertService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activities.Clear();
        if(User is not null)
        {
            var activities = User.UserActivities;
            foreach (var activity in activities)
            {
                if (activity.ProjectId == null)
                {
                    Activities.Add(activity);
                }
            }
            Projects.Clear();
            var projects = await _projectFacade.GetAsync();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }
    }

    [RelayCommand]
    private async Task SaveProjectAsync()
    {
        if (Project is not null && User is not null && SelectedActivity is not null)
        {
            if (Project.Name == "")
            {
                await _alertService.DisplayAsync("Name Error", "Name value not inserted");
            }
            else
            {
                await _projectFacade.SaveAsync(Project, User.Id);
                await _activityFacade.AddActivityToProjectAsync(SelectedActivity, User.Id, Project.Id);
                Project.ProjectActivities.Add(SelectedActivity);
                User.UserProjects.Add(_projectModelMapper.MapToListModel(Project));
                MessengerService.Send(new ProjectCreateMessage());
                _navigationService.SendBackButtonPressed();
            }
        }
    }

    [RelayCommand]
    private async Task JoinProjectAsync()
    {
        if (SelectedProjectListModel is not null && User is not null && SelectedActivity is not null)
        {
            await _activityFacade.AddActivityToProjectAsync(SelectedActivity, User.Id, SelectedProjectListModel.Id);

            User.UserProjects.Add(SelectedProjectListModel);
            MessengerService.Send(new ProjectCreateMessage());
            _navigationService.SendBackButtonPressed();

        }
    }
}