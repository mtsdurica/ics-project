using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;


namespace ProjectICS.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>,
    IProjectModelMapper
{
    private readonly IActivityModelMapper _activityModelMapper;

    public ProjectModelMapper(IActivityModelMapper activityModelMapper)
    {
        _activityModelMapper = activityModelMapper;
    }

    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null
            ? ProjectListModel.Empty
            : new ProjectListModel
            {
                Id = entity.Id,
                Name = entity.Name
            };

    public ProjectListModel MapToListModel(ProjectDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            Name = detailModel.Name
        };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null
            ? ProjectDetailModel.Empty
            : new ProjectDetailModel()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                User = entity.User,
                ProjectActivities = _activityModelMapper.MapToListModel(entity.ProjectActivities).ToObservableCollection()
            };

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
        => throw new NotImplementedException("This method is unsupported. Use the other overload.");
    
    public  ProjectEntity MapToEntity(ProjectDetailModel model, Guid userId)
        => new()
        {
            Id = model.Id,
            UserId = userId,
            Name = model.Name
        };


    public ProjectEntity MapToEntity(ProjectListModel model, Guid userId)
        => new()
        {
            Id = model.Id,
            UserId = userId,
            Name = model.Name
        };
}