using ProjectICS.BL.Facades;
using ProjectICS.BL.Models;
using ProjectICS.Common.Tests;
using ProjectICS.Common.Tests.TestingSeeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ProjectICS.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ProjectICS.BL.Tests;

public sealed class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, ActivityEntityMapper);
    }
    

    [Fact]
    public async Task Create_WithoutProject_DoesNotThrow()
    {
        var model = new ActivityDetailModel()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = @"Activity 1",
            Type = @"Activity Type 1",
            Description = @"Testing Activity",
            StartTime = new DateTime(2022, 1, 1, 0, 0, 0), 
            EndTime = new DateTime(2022, 1, 1, 1, 0, 0)
        };

        var _ = await _activityFacadeSUT.SaveAsync(model, UserEntitySeeds.SeededUser2.Id);
    }

    [Fact]
    public async Task GetAll_Single_SeededJamming()
    {
        var activities = await _activityFacadeSUT.GetAsync();
        var activity = activities.Single(i => i.Id == ActivityEntitySeeds.Homework.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivityEntitySeeds.Homework), activity);
    }

    [Fact]
    public async Task GetById_SeededJamming()
    {
        var activity = await _activityFacadeSUT.GetAsync(ActivityEntitySeeds.Homework.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivityEntitySeeds.Homework), activity);
    }

    [Fact]
    public async Task GetById_NonExisting()
    {
        var activity = await _activityFacadeSUT.GetAsync(ActivityEntitySeeds.EmptyActivityEntity.Id);

        Assert.Null(activity);
    }

    [Fact]
    public async Task SeededHomework_DeleteById_Deleted()
    {
        await _activityFacadeSUT.DeleteAsync(ActivityEntitySeeds.Homework.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Activities.AnyAsync(i => i.Id == ActivityEntitySeeds.Homework.Id));
    }

    [Fact]
    public async Task NewActivity_ActivityAdded()
    {
        Guid entityId = Guid.NewGuid();
        var activity = new ActivityDetailModel()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = "Basketball",
            Description = "Bball practice",
            Type = "Sports",
            StartTime = new DateTime(2021, 5, 14, 18, 0, 0),
            EndTime = new DateTime(2021, 5, 14, 20, 0, 0)

        };

        activity = await _activityFacadeSUT.SaveAsync(activity, UserEntitySeeds.SeededUser2.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities.SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    }

    [Fact]
    public async Task HomeworkActivity_ActivityUpdated()
    {
        var activity = new ActivityDetailModel()
        {
            Id = ActivityEntitySeeds.Homework.Id,
            UserId = UserEntitySeeds.SeededUser2.Id,
            Name = ActivityEntitySeeds.Homework.Name,
            Description = ActivityEntitySeeds.Homework.Description,
            Type = ActivityEntitySeeds.Homework.Type,
            StartTime = ActivityEntitySeeds.Homework.StartTime,
            EndTime = ActivityEntitySeeds.Homework.EndTime
        };

        await _activityFacadeSUT.UpdateAsync(activity);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities.SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    }

}