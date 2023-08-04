using ProjectICS.App.ViewModels;

namespace ProjectICS.App.Views.User;

public partial class UserMainPageView
{
    public UserMainPageView(UserMainPageViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}