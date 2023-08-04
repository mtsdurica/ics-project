using ProjectICS.App.ViewModels;

namespace ProjectICS.App.Views.Activity;

public partial class ActivityDetailView
{
    public ActivityDetailView(ActivityDetailViewModel viewModel)
        :base(viewModel)
    {
        InitializeComponent();
    }
}