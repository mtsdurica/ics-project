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

public class DbContextProjectEntityTests : DbContextTestsBase
{
    public DbContextProjectEntityTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_ProjectWithoutActivitiesOrUsers()
    {
        var entity = ProjectEntitySeeds.EmptyProject with
        {
            Id = Guid.NewGuid(),
            UserId = UserEntitySeeds.SeededUser.Id,
            Name = "Music"
        };

        ProjectICSDbContextSUT.Projects.Add(entity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Projects.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equals(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_ProjectWithActivity()
    {
        Guid entityId = Guid.NewGuid();
        var entity = ProjectEntitySeeds.EmptyProject with
        {
            Id = entityId,
            Name = "History",
            UserId = UserEntitySeeds.SeededUser.Id,
            ProjectActivities = new List<ActivityEntity>
            {
                ActivityEntitySeeds.EmptyActivityEntity with
                {
                    Id = Guid.NewGuid(),
                    ProjectId = entityId,
                    UserId = UserEntitySeeds.SeededUser.Id,
                    Name = "WW1",
                    Description = "WW1 fun facts",
                    Type = "Study"
                }
            }
        };

        ProjectICSDbContextSUT.Projects.Add(entity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Projects.Include(i => i.ProjectActivities).SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equals(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_ProjectWithUser()
    {
 
        var entity = ProjectEntitySeeds.EmptyProject with
        {
            Id = Guid.NewGuid(),
            Name = "History",
            UserId = UserEntitySeeds.SeededUser.Id 
        };

        ProjectICSDbContextSUT.Projects.Add(entity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Projects.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetAll_SeededProject()
    {
        var result = await ProjectICSDbContextSUT.Projects.ToArrayAsync();
        Assert.Contains(result, i => i.Id == ProjectEntitySeeds.SchoolProj.Id);
    }

    [Fact]
    public async Task GetById_SeededProject()
    {
        var result = await ProjectICSDbContextSUT.Projects.SingleAsync(i => i.Id == ProjectEntitySeeds.SchoolProj.Id);
        Assert.Equivalent(ProjectEntitySeeds.SchoolProj, result);
    }

    [Fact]
    public async Task GetById_IncludingActivity()
    {
        var entity = await ProjectICSDbContextSUT.Projects
            .Include(i => i.ProjectActivities)
            .SingleAsync(i => i.Id == ProjectEntitySeeds.SchoolProj.Id);

        DeepAssert.Equal(ProjectEntitySeeds.SchoolProj, entity);
    }

    [Fact]
    public async Task Update_SeededProject()
    {
        var baseEntity = ProjectEntitySeeds.SeededProjectUpdate;
        var updatedEntity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated"
            };

        ProjectICSDbContextSUT.Projects.Update(updatedEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Projects.SingleAsync(i => i.Id == updatedEntity.Id);
        DeepAssert.Equal(updatedEntity, actualEntity);
    }

    [Fact]
    public async Task Remove_SeededProject()
    {
        var baseEntity = ProjectEntitySeeds.SeededProjectDelete;

        ProjectICSDbContextSUT.Projects.Remove(baseEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Projects.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task RemoveById_SeededProject()
    {
        var baseEntity = ProjectEntitySeeds.SeededProjectDelete;

        ProjectICSDbContextSUT.Remove(ProjectICSDbContextSUT.Projects.Single(i => i.Id == baseEntity.Id));
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Projects.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task Remove_ProjectWithActivity()
    {
        var baseEntity = ProjectEntitySeeds.SchoolProj;

        ProjectICSDbContextSUT.Projects.Remove(baseEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Projects.AnyAsync(i => i.Id == baseEntity.Id));
        Assert.False(await ProjectICSDbContextSUT.Activities
            .AnyAsync(i => baseEntity.ProjectActivities.Select(ActivityEntity => ActivityEntity.Id).Contains(i.Id)));
    }

}