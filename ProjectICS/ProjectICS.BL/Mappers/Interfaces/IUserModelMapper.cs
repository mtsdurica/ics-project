using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;

namespace ProjectICS.BL.Mappers.Interfaces;

public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel>
{
    
}