using ProjectICS.App.ViewModels;

namespace ProjectICS.App.Views.Activity;

public partial class ActivityCreateView
{
    public ActivityCreateView(ActivityCreateViewModel viewModel)
        :base(viewModel)
    {
        InitializeComponent();
    }
}