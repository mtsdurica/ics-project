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

public class ProjectFacadeTests : FacadeTestsBase
{
    private readonly IProjectFacade _projectFacadeSUT;

    public ProjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _projectFacadeSUT = new ProjectFacade(UnitOfWorkFactory, ProjectModelMapper, UserFacade, ActivityFacade, UserModelMapper, ActivityModelMapper, ProjectEntityMapper);
    }

    [Fact]
    public async Task Create_WithoutUsersNorActivities()
    {
        var model = new ProjectDetailModel()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = "Project 1",
        };
        
        var returnedModel = await _projectFacadeSUT.SaveAsync(model, UserEntitySeeds.SeededUser2.Id);

        FixIds(model, returnedModel);
        DeepAssert.Equals(model, returnedModel);
    }

    [Fact]
    public async Task Create_WithNonExistingUserThrows()
    {
        
        var model = new ProjectDetailModel()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = "Project 2"
        };
        await Assert.ThrowsAnyAsync<DbUpdateException>(() => _projectFacadeSUT.SaveAsync(model, Guid.NewGuid()));
    }

    [Fact]
    public async Task GetById_FromSeeded_DoesNotThrow()
    {
        var detailModel = ProjectModelMapper.MapToDetailModel(ProjectEntitySeeds.SchoolProj);

        var returnedModel = await _projectFacadeSUT.GetAsync(detailModel.Id);

        DeepAssert.Equal(detailModel, returnedModel);
    }
    [Fact]
    public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
    {
        //Arrange
        var listModel = ProjectModelMapper.MapToListModel(ProjectEntitySeeds.SchoolProj);

        //Act
        var returnedModel = await _projectFacadeSUT.GetAsync();

        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = ProjectModelMapper.MapToDetailModel(ProjectEntitySeeds.SchoolProj);
        detailModel.Name = "Changed project name";

        //Act & Assert
        await _projectFacadeSUT.UpdateAsync(detailModel);
    }

    [Fact]
    public async Task Update_Name_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = ProjectModelMapper.MapToDetailModel(ProjectEntitySeeds.SchoolProj);
        detailModel.Name = "Changed project name 1";

        //Act
        await _projectFacadeSUT.UpdateAsync(detailModel);

        //Assert
        var returnedModel = await _projectFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equals(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveUsers_FromSeeded_CheckNotUpdated()
    {
        //Arrange
        var detailModel = ProjectModelMapper.MapToDetailModel(ProjectEntitySeeds.SchoolProj);
        detailModel.ProjectUsers.Clear();

        //Act
        await _projectFacadeSUT.UpdateAsync(detailModel);

        //Assert
        var returnedModel = await _projectFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equals(ProjectModelMapper.MapToDetailModel(ProjectEntitySeeds.SchoolProj), returnedModel);
    }

    [Fact]
    public async Task SeededSchoolProj_DeleteById_Deleted()
    {
        await _projectFacadeSUT.DeleteAsync(ProjectEntitySeeds.SchoolProj.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Projects.AnyAsync(i => i.Id == ProjectEntitySeeds.SchoolProj.Id));
    }

    private static void FixIds(ProjectDetailModel expectedM, ProjectDetailModel returnedM)
    {
        returnedM.Id = expectedM.Id;

        foreach (var userModel in returnedM.ProjectUsers)
        {
           

            var userDetailModel = expectedM.ProjectUsers.FirstOrDefault(i => 
                i.Name == userModel.Name
                && i.Surname == userModel.Surname
                && i.PictureLink == userModel.PictureLink);

            if (userDetailModel != null)
            {
                userModel.Id = userDetailModel.Id;
            }
        }

        foreach (var activityModel in returnedM.ProjectActivities)
        {
            var activityDetailModel = expectedM.ProjectActivities.FirstOrDefault(i => i.Id == activityModel.Id
            && i.Name == activityModel.Name);

            if (activityDetailModel != null)
            {
                activityModel.Id = activityDetailModel.Id;

            }
        }
    }
}