using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;
using System.ComponentModel;

namespace ProjectICS.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>, IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProjectId = entity.ProjectId,
                Name = entity.Name, 
                Type = entity.Type                
            };

    public ActivityListModel MapToListModel(ActivityDetailModel detailModel)
    => new()
    {   Id = detailModel.Id,
        UserId = detailModel.UserId,
        ProjectId = detailModel.ProjectId,
        Name = detailModel.Name,
        Type = detailModel.Type
    };


    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                Description = entity.Description is null ? "" : entity.Description,
                Type = entity.Type,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime
            };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
        => throw new NotImplementedException("This method is unsupported. Use the other overload.");

    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid userId)
        => new()
        {
            Id = model.Id,
            UserId = userId,
            ProjectId = model.ProjectId,
            Name = model.Name,
            Type = model.Type,
            Description = model.Description,
            StartTime = model.StartTime,
            EndTime = model.EndTime
        };

    public ActivityEntity MapToEntity(ActivityListModel model, Guid userId)
        => new()
        {
            Id = model.Id,
            UserId = userId,
            Name = model.Name,
            Type = model.Type
        };

    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid userId, Guid projectId)
        => new()
        {
            Id = model.Id,
            UserId = userId,
            ProjectId = projectId,
            Name = model.Name,
            Type = model.Type,
            Description = model.Description,
            StartTime = model.StartTime,
            EndTime = model.EndTime
        };

    public ActivityEntity MapToEntity(ActivityListModel model, Guid userId, Guid projectId)
        => new()
        {
            Id = model.Id,
            UserId = userId,
            ProjectId = projectId,
            Name = model.Name,
            Type = model.Type
        };

}