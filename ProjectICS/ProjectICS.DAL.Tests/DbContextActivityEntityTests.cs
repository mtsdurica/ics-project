using System;
using System.Threading.Tasks;
using ProjectICS.DAL.Factories;
using ProjectICS.DAL.Entities;
using ProjectICS.Common.Tests;
using ProjectICS.Common.Tests.TestingFactories;
using ProjectICS.Common.Tests.TestingSeeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace ProjectICS.DAL.Tests;

public class DbContextActivityEntityTests : DbContextTestsBase
{
    public DbContextActivityEntityTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Activity()
    {
        var entity = ActivityEntitySeeds.EmptyActivityEntity with
        {
            Name = "Playing guitar",
            UserId = UserEntitySeeds.SeededUser.Id,
            Type = "Music",
            Description = "Daily guitar playing",
        };

        ProjectICSDbContextSUT.Activities.Add(entity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);
        Assert.Equivalent(entity, actualEntity);
    }

    [Fact]
    public async Task GetAll_SeededActivity()
    {
        var results = await ProjectICSDbContextSUT.Activities.ToArrayAsync();
        Assert.Contains(results, i => i.Id == ActivityEntitySeeds.Homework.Id);
    }

    [Fact]
    public async Task GetById_SeededActivity()
    {
        var result = await ProjectICSDbContextSUT.Activities.SingleAsync(i => i.Id == ActivityEntitySeeds.Homework.Id);

        Assert.Equivalent(ActivityEntitySeeds.Homework, result);
    }

    [Fact]
    public async Task Update_SeededActivity()
    {
        var baseEntity = ActivityEntitySeeds.HomeworkUpdate;
        var updatedEntity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated"
            };

        ProjectICSDbContextSUT.Activities.Update(updatedEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == updatedEntity.Id);
        DeepAssert.Equal(updatedEntity, actualEntity);
    }


    [Fact]
    public async Task Remove_SeededActivity()
    {
        var baseEntity = ActivityEntitySeeds.HomeworkDelete;
        
        ProjectICSDbContextSUT.Activities.Remove(baseEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Activities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task RemoveById_SeededActivity()
    {
        var baseEntity = ActivityEntitySeeds.Homework;

        ProjectICSDbContextSUT.Remove(ProjectICSDbContextSUT.Activities.Single(i => i.Id == baseEntity.Id));
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Activities.AnyAsync(i => i.Id == baseEntity.Id));
    }
}