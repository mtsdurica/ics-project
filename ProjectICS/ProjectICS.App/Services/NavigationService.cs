using ProjectICS.App.Views.User;
using ProjectICS.App.Models;
using ProjectICS.App.ViewModels;
using ProjectICS.App.Views.Activity;
using ProjectICS.App.Views.Project;

namespace ProjectICS.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//users", typeof(MainPageView), typeof(MainPageViewModel)),
        new("//users/edit", typeof(UserEditView), typeof(UserEditViewModel)),
        new("//users//mPage", typeof(UserMainPageView), typeof(UserMainPageViewModel)),
        new("//users//mPage//profile", typeof(UserProfileView), typeof(UserProfileViewModel)),
        new("//users//mPage//createProject", typeof(ProjectCreateView), typeof(ProjectCreateViewModel)),
        new("//users//mPage//project", typeof(ProjectDetailView), typeof(ProjectDetailViewModel)),
        new("//users//mPage//createActivity", typeof(ActivityCreateView), typeof(ActivityCreateViewModel)),
        new("//users//mPage//activity", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),
    };

    public async Task GoToAsync<TViewModel>() where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }
    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel 
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
}