using ProjectICS.BL.Facades;
using ProjectICS.BL.Models;
using ProjectICS.Common.Tests;
using ProjectICS.Common.Tests.TestingSeeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ProjectICS.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ProjectICS.BL.Tests;

public class UserFacadeTests : FacadeTestsBase
{
    private readonly IUserFacade _userFacadeSUT;

    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper, UserEntityMapper);
    }

    [Fact]
    public async Task Create_WithoutProjectsNorActivities()
    {
        var model = new UserDetailModel()
        {
            Name = "User",
            Surname = "1",
            PictureLink = "picturelink"

        };

        var returnedModel = await _userFacadeSUT.SaveAsync(model);

        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Create_WithNonExistingActivityThrows()
    {
        Guid modelId = Guid.NewGuid();
        var model = new UserDetailModel()
        {
            Id = modelId,
            Name = "User",
            Surname = "2",
            PictureLink = "picturelink",

            UserActivities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    UserId = modelId,
                    Name = "Activity 1",
                    Type = "Type 1"
                }
            }
        };
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));

    }

    [Fact]
    public async Task GetById_FromSeeded_DoesNotThrow()
    {
        var detailModel = UserModelMapper.MapToDetailModel(UserEntitySeeds.SeededUser);

        var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);

        DeepAssert.Equals(detailModel, returnedModel);
    }
    [Fact]
    public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
    {
        //Arrange
        var listModel = UserModelMapper.MapToListModel(UserEntitySeeds.SeededUser);

        //Act
        var returnedModel = await _userFacadeSUT.GetAsync();

        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserEntitySeeds.SeededUser);
        detailModel.Name = "Changed user name";
        detailModel.Surname = "Changed user surname";

        //Act & Assert
        await _userFacadeSUT.SaveAsync(detailModel with { UserProjects = default, UserActivities = default });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserEntitySeeds.SeededUser);
        detailModel.Name = "Changed user name 1";

        //Act
        await _userFacadeSUT.SaveAsync(detailModel with { UserProjects = default, UserActivities = default });

        //Assert
        var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equals(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveActivities_FromSeeded_CheckNotUpdated()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserEntitySeeds.SeededUser);
        detailModel.UserActivities.Clear();

        //Act
        await _userFacadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equals(UserModelMapper.MapToDetailModel(UserEntitySeeds.SeededUser), returnedModel);
    }

    [Fact]
    public async Task SeededSeededUser_DeleteById_Deleted()
    {
        await _userFacadeSUT.DeleteAsync(UserEntitySeeds.SeededUser.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Users.AnyAsync(i => i.Id == UserEntitySeeds.SeededUser.Id));
    }

    private static void FixIds(UserDetailModel expectedM, UserDetailModel returnedM)
    {
        returnedM.Id = expectedM.Id;

        foreach (var projectModel in returnedM.UserProjects)
        {
            var projectDetailModel = expectedM.UserProjects.FirstOrDefault(i =>
                i.Name == projectModel.Name);

            if (projectDetailModel != null)
            {
                projectModel.Id = projectDetailModel.Id;
            }
        }

        foreach (var activityModel in returnedM.UserActivities)
        {
            var activityDetailModel = expectedM.UserActivities.FirstOrDefault(i => i.Id == activityModel.Id
                && i.Name == activityModel.Name);

            if (activityDetailModel != null)
            {
                activityModel.Id = activityDetailModel.Id;

            }
        }
    }
}