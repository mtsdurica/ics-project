using ProjectICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectICS.Common.Tests.TestingSeeds;

public static class ActivityEntitySeeds
{
    public static readonly ActivityEntity EmptyActivityEntity = new ActivityEntity()
    {
        Id = default,
        UserId = default,
        Name = default!,
        Type = default!,
        Description = default,
        StartTime = default,
        EndTime = default,
    };

    public static readonly ActivityEntity Homework = new()
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        UserId = UserEntitySeeds.SeededUser.Id,
        Name = "Doing homework",
        Type = "Education",
        Description = "History session",
        StartTime = new DateTime(2023, 5, 2, 18, 0, 0),
        EndTime = new DateTime(2023, 5, 2, 20, 0, 0)
    };

    public static readonly ActivityEntity HomeworkUpdate = Homework with { Id = Guid.Parse("ee5e7f34-0430-4193-88fb-1dae5b3e48c7") };

    public static readonly ActivityEntity HomeworkDelete = Homework with { Id = Guid.Parse("3510e84f-3787-4584-a05f-4b656d2da520") };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>().HasData(
            Homework,
            HomeworkUpdate,
            HomeworkDelete
            );
    }
}