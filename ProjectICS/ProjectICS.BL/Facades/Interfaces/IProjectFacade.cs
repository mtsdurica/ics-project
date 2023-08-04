using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;

namespace ProjectICS.BL.Facades;

public interface IProjectFacade : IFacade<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    Task<ProjectDetailModel> SaveAsync(ProjectDetailModel model, Guid userId);
    Task<ProjectDetailModel> GetProjectDetailsAsync(Guid id);
    Task<ProjectDetailModel> GetProjByIdAsync(Guid id);
    Task UpdateAsync(ProjectDetailModel model);
}