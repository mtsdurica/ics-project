using System;
using ProjectICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectICS.DAL.Seeds;

public static class ActivitySeed
{
    public static ActivityEntity JammingWithFriends = new()
    {
        Id = Guid.Parse("06b8e2cf-ea03-4495-a3e4-aa0291fe9c75"),
        Description = "Jamming with Pete, Robert and Stacy",
        Name = "JammingWithFriends",
        Type = "Socializing",
        UserId = UserSeed.Freddie.Id,
        ProjectId = ProjectSeed.School.Id,
        StartTime = new DateTime(2023, 1, 1, 20, 0, 0),
        EndTime = new DateTime(2023, 1, 1, 22, 0, 0)
    };

    public static ActivityEntity BasketballPractice = new()
    {
        Id = Guid.Parse("a216de58-7e26-42de-84a4-8c6328b17966"),
        Description = "Basketball practice with the Bulls",
        Name = "BasketballPractice",
        Type = "Sport",
        UserId = UserSeed.Michael.Id,
        ProjectId = ProjectSeed.Bulls.Id,
        StartTime = new DateTime(2023, 1, 2, 16, 0, 0),
        EndTime = new DateTime(2023, 1, 2, 18, 0, 0)
    };

    public static ActivityEntity CookingLesson = new()
    {
        Id = Guid.Parse("2d365826-1572-4fc1-92bb-b8198c3422da"),
        Description = "Cooking lesson at the culinary school",
        Name = "CookingLesson",
        Type = "Education",
        UserId = UserSeed.Billy.Id,
        ProjectId = ProjectSeed.School.Id,
        StartTime = new DateTime(2023, 3, 8, 14, 0, 0),
        EndTime = new DateTime(2023, 3, 8, 16, 0, 0)
    };

    public static ActivityEntity Coffee = new()
    {
        Id = Guid.Parse("1916d945-5408-4cdb-8238-0929ee299859"),
        Description = "Coffee with Darcy",
        Name = "Coffee",
        Type = "Socializing",
        UserId = UserSeed.James.Id,
        ProjectId = ProjectSeed.Pumpkins.Id,
        StartTime = new DateTime(2023, 1, 1, 20, 0, 0),
        EndTime = new DateTime(2023, 1, 1, 22, 0, 0)
    };

    public static ActivityEntity BandPractice = new()
    {
        Id = Guid.Parse("1915e945-5409-4cdb-8238-0129ee291851"),
        Description = "Coffee with Darcy",
        Name = "Band practice with the Pumpkins",
        Type = "Music",
        UserId = UserSeed.Billy.Id,
        ProjectId = ProjectSeed.Pumpkins.Id,
        StartTime = new DateTime(2023, 1, 1, 20, 0, 0),
        EndTime = new DateTime(2023, 1, 1, 22, 0, 0)
    };

    public static ActivityEntity TwoDaysBack = new()
    {
        Id = Guid.Parse("B89DBA99-7465-4E1B-8E19-F8C0D58D3A6F"),
        Description = "foo",
        Name = "TwoDaysBack",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 5, 14, 0, 0, 0),
        EndTime = new DateTime(2023, 5, 14, 1, 0, 0)
    };

    public static ActivityEntity TwoDaysBackLater = new()
    {
        Id = Guid.Parse("E1D451A8-B28A-4ECD-83BF-4728479B5297"),
        Description = "foo",
        Name = "TwoDaysBackLater",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 5, 14, 3, 0, 0),
        EndTime = new DateTime(2023, 5, 14, 4, 0, 0)
    };

    public static ActivityEntity TwoDaysBackEvenLater = new()
    {
        Id = Guid.Parse("1916d945-5408-4cdb-83BF-4728479B5297"),
        Description = "foo",
        Name = "TwoDaysBackEvenLater",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 5, 14, 20, 0, 0),
        EndTime = new DateTime(2023, 5, 14, 22, 0, 0)
    };

    public static ActivityEntity NextWeek = new()
    {
        Id = Guid.Parse("79FF2C51-A237-414B-A9ED-D2291F66F780"),
        Description = "foo",
        Name = "NextWeek",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 5, 24, 0, 0, 0),
        EndTime = new DateTime(2023, 5, 24, 3, 0, 0)
    };

    public static ActivityEntity NextWeekLater = new()
    {
        Id = Guid.Parse("79FF2C51-A237-414B-8238-0929ee299859"),
        Description = "foo",
        Name = "NextWeekLater",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 5, 24, 20, 0, 0),
        EndTime = new DateTime(2023, 5, 24, 22, 0, 0)
    };

    public static ActivityEntity LastMonth = new()
    {
        Id = Guid.Parse("48667ACA-7E09-4DE4-BB1C-EB887DD36801"),
        Description = "foo",
        Name = "LastMonth",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 4, 2, 0, 0, 0),
        EndTime = new DateTime(2023, 4, 2, 1, 0, 0)
    };

    public static ActivityEntity LastMonthEarlier = new()
    {
        Id = Guid.Parse("5D3C4DAC-3E68-440B-AC3F-404D2F55E66A"),
        Description = "foo",
        Name = "LastMonthEarlier",
        Type = "foo",
        UserId = UserSeed.Michael.Id,
        StartTime = new DateTime(2023, 4, 1, 20, 0, 0),
        EndTime = new DateTime(2023, 4, 1, 22, 0, 0)
    };



    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityEntity>().HasData(
            JammingWithFriends,
            BasketballPractice,
            CookingLesson,
            Coffee,
            BandPractice,
            TwoDaysBack,
            TwoDaysBackLater,
            TwoDaysBackEvenLater,
            NextWeek,
            NextWeekLater,
            LastMonth,
            LastMonthEarlier
        );
}