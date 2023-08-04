using ProjectICS.App.ViewModels;

namespace ProjectICS.App.Views.User;

public partial class UserProfileView
{
    public UserProfileView(UserProfileViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}