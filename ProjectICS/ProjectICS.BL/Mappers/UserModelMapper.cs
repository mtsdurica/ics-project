using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;


namespace ProjectICS.BL.Mappers;

public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>,
    IUserModelMapper
{
    private readonly IProjectModelMapper _projectModelMapper;
    private readonly IActivityModelMapper _activityModelMapper;

    public UserModelMapper(IProjectModelMapper projectModelMapper, IActivityModelMapper activityModelMapper)
    {
        _projectModelMapper = projectModelMapper;
        _activityModelMapper = activityModelMapper;
    }

    public override UserListModel MapToListModel(UserEntity? entity)
        => entity is null
            ? UserListModel.Empty
            : new UserListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                PictureLink = entity.PictureLink

            };

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
        => entity is null
            ? UserDetailModel.Empty
            : new UserDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                PictureLink = entity.PictureLink,
                UserActivities = _activityModelMapper.MapToListModel(entity.UserActivities).ToObservableCollection(),
                UserProjects = _projectModelMapper.MapToListModel(entity.UserProjects).ToObservableCollection()
            };

    public override UserEntity MapToEntity(UserDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Surname = model.Surname,
            PictureLink = model.PictureLink
        };

}