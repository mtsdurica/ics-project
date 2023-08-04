using System;
using ProjectICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectICS.DAL.Seeds;

public static class ProjectSeed
{
    public static readonly ProjectEntity School = new()
    {
        Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        UserId = UserSeed.Freddie.Id,
        Name = "School"
    };

    public static readonly ProjectEntity Bulls = new()
    {
        Id = Guid.Parse(input: "facde0cd-effe-443f-baf6-3d96cc2cbf2e"),
        UserId = UserSeed.Freddie.Id,
        Name = "1995 Bulls"
    };

    public static readonly ProjectEntity Pumpkins = new()
    {
        Id = Guid.Parse(input: "facde1cd-effe-433f-baf6-3d96cc2cbf2e"),
        UserId = UserSeed.Billy.Id,
        Name = "Smashing Pumpkins"
    };

    static ProjectSeed()
    {
        School.ProjectUsers.Add(UserSeed.Freddie);
        School.ProjectActivities.Add(ActivitySeed.JammingWithFriends);

    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ProjectEntity>().HasData(
            School with { ProjectUsers = Array.Empty<UserEntity>(), ProjectActivities = Array.Empty<ActivityEntity>() },
            Bulls with { ProjectUsers = Array.Empty<UserEntity>(), ProjectActivities = Array.Empty<ActivityEntity>() },
            Pumpkins with { ProjectUsers = Array.Empty<UserEntity>(), ProjectActivities = Array.Empty<ActivityEntity>() }
        );

}