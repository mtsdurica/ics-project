using ProjectICS.App.ViewModels;

namespace ProjectICS.App.Views.User;

public partial class UserEditView
{
	public UserEditView(UserEditViewModel viewModel)
	    :base(viewModel)
	{
		InitializeComponent();
	}
}