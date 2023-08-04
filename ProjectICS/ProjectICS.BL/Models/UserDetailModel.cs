using System.Collections.ObjectModel;
using ProjectICS.DAL.Entities;

namespace ProjectICS.BL.Models;

public record UserDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? PictureLink { get; set; }

    public ObservableCollection<ActivityListModel> UserActivities { get; init; } = new();
    public ObservableCollection<ProjectListModel> UserProjects { get; init; } = new();

    public static UserDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty
    };

}