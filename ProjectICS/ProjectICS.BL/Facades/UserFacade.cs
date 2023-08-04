using ProjectICS.BL.Mappers;
using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;
using ProjectICS.DAL.Mappers;
using ProjectICS.DAL.Repositories;
using ProjectICS.DAL.UnitOfWork;

namespace ProjectICS.BL.Facades;

public class UserFacade : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    private readonly IUserModelMapper _modelMapper;
    private readonly UserEntityMapper _entityMapper;
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IUserModelMapper modelMapper, UserEntityMapper entityMapper) : base(unitOfWorkFactory, modelMapper)
    {
        _modelMapper = modelMapper;
        _entityMapper = entityMapper;
    }

    protected override string IncludesNavigationPathDetail =>
        $"{nameof(UserEntity.UserActivities)}.{nameof(ActivityEntity.Project)}";

    public async Task UpdateAsync(UserDetailModel model)
    {
        UserEntity searchedEntity = _modelMapper.MapToEntity(model);
        UserEntity updatedEntity = _modelMapper.MapToEntity(model);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<UserEntity> repository =
            uow.GetRepository<UserEntity, UserEntityMapper>();

        if (await repository.ExistsAsync(searchedEntity))
        {
            searchedEntity = await repository.UpdateAsync(searchedEntity);
            _entityMapper.MapToExistingEntity(searchedEntity, updatedEntity);
            await uow.CommitAsync();
        }
    }
}