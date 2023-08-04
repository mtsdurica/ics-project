using System;
using System.Collections.ObjectModel;
using ProjectICS.DAL.Entities;


namespace ProjectICS.BL.Models;

public record ProjectDetailModel : ModelBase
{
    public required string Name { get; set; }
    public ObservableCollection<UserListModel> ProjectUsers { get; init; } = new();
    public UserEntity? User { get; set; }
    public required Guid UserId { get; set; }
    public ObservableCollection<ActivityListModel> ProjectActivities { get; init; } = new();

    public static ProjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        UserId = Guid.Empty,
        Name = string.Empty
    };
}