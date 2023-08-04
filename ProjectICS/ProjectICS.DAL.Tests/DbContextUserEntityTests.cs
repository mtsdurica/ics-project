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

public class DbContextUserEntityTests : DbContextTestsBase
{
    public DbContextUserEntityTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_UserWithoutActivitiesOrProjects()
    {
        var entity = new UserEntity()
        {
            Id = Guid.Parse("a7b43393-c7b5-42a1-8dc9-7454a4ef8540"),
            Name = "Billy",
            Surname = "Corgan",
            PictureLink = "https://www.nme.com/wp-content/uploads/2020/09/smashing-pumpkins-getty-images-pedro-gomes-redferns-.jpg"
        };

        ProjectICSDbContextSUT.Users.Add(entity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntities);
    }
    
    [Fact]
    public async Task AddNew_UserWithActivity()
    {
        Guid entityId = Guid.NewGuid();
        var entity = UserEntitySeeds.EmptyUser with
        {   
            Id = entityId,
            Name = "James",
            Surname = "Iha",
            PictureLink = "https://media.stubhubstatic.com/stubhub-catalog/d_defaultLogo.jpg/t_f-fs-0fv,q_auto:low,f_auto,c_fill,$w_280_mul_3,$h_180_mul_3/performer/724647/qkkb3sxcpdk0s5dyjrjm",
            UserActivities = new List<ActivityEntity>
            {
                ActivityEntitySeeds.EmptyActivityEntity with
                {
                    Id = Guid.NewGuid(),
                    UserId = entityId,
                    Name = "Guitar lesson",
                    Description = "Downpicking practice",
                    Type = "Music"
                }
            }
            
        };

        ProjectICSDbContextSUT.Users.Add(entity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.Include(i => i.UserActivities).SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equals(entity, actualEntity);
    }
    

    [Fact]
    public async Task Update_SeededUser()
    {
        var baseEntity = UserEntitySeeds.SeededUserUpdate;
        var updatedEntity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated",
                Surname = baseEntity.Surname + "Updated",
            };

        ProjectICSDbContextSUT.Users.Update(updatedEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == updatedEntity.Id);
        DeepAssert.Equal(updatedEntity, actualEntity);
    }

    [Fact]
    public async Task GetAll_SeededUser()
    {
        var results = await ProjectICSDbContextSUT.Users.ToArrayAsync();
        Assert.Contains(results, i => i.Id == UserEntitySeeds.SeededUser.Id);
    }

    

    [Fact]
    public async Task GetById_SeededUser()
    {
        var result = await ProjectICSDbContextSUT.Users.SingleAsync(i => i.Id == UserEntitySeeds.SeededUser.Id);

        Assert.Equivalent(UserEntitySeeds.SeededUser, result);
    }

    [Fact]
    public async Task Remove_SeededUser()
    {
        var baseEntity = UserEntitySeeds.SeededUserDelete;

        ProjectICSDbContextSUT.Users.Remove(baseEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task RemoveById_SeededUser()
    {
        var baseEntity = UserEntitySeeds.SeededUser;

        ProjectICSDbContextSUT.Remove(ProjectICSDbContextSUT.Users.Single(i => i.Id == baseEntity.Id));
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task Remove_UserWithActivity()
    {
        var baseEntity = UserEntitySeeds.SeededUser;

        ProjectICSDbContextSUT.Users.Remove(baseEntity);
        await ProjectICSDbContextSUT.SaveChangesAsync();

        Assert.False(await ProjectICSDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
        Assert.False(await ProjectICSDbContextSUT.Activities
            .AnyAsync(i => baseEntity.UserActivities.Select(ActivityEntity => ActivityEntity.Id).Contains(i.Id)));
    }

}