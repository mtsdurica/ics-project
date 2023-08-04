using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;


namespace ProjectICS.BL.Mappers.Interfaces;

public interface IProjectModelMapper : IModelMapper<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    ProjectEntity MapToEntity(ProjectDetailModel model, Guid projectId);
    ProjectEntity MapToEntity(ProjectListModel model, Guid projectId);
    ProjectListModel MapToListModel(ProjectDetailModel detailModel);
}