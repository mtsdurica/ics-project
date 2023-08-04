using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;

namespace ProjectICS.BL.Mappers.Interfaces;

public interface IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    ActivityEntity MapToEntity(ActivityDetailModel model, Guid userId);
    ActivityEntity MapToEntity(ActivityListModel model, Guid userId);
    ActivityEntity MapToEntity(ActivityDetailModel model, Guid userId, Guid projectId);
    ActivityEntity MapToEntity(ActivityListModel model, Guid userId, Guid projectId);
    ActivityListModel MapToListModel(ActivityDetailModel detailModel);
}