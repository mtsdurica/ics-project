using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;


namespace ProjectICS.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid userId);
    Task AddActivityToProjectAsync(ActivityListModel model, Guid userId, Guid projectId);
    Task<IEnumerable<ActivityEntity>> GetByProjectIdAsync(Guid projectId);
    Task UpdateAsync(ActivityDetailModel model);
    Task<IEnumerable<ActivityListModel>> GetFilteredActivitiesAsync(Guid userId, string firstSelected, string secondSelected);
}