using ProjectICS.App.ViewModels;

namespace ProjectICS.App.Views.Project;

public partial class ProjectDetailView
{
    public ProjectDetailView(ProjectDetailViewModel viewModel)
        :base(viewModel)
    {
        InitializeComponent();
    }
}