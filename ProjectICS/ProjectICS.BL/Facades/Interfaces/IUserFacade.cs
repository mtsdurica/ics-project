using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;

namespace ProjectICS.BL.Facades;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
{
    Task UpdateAsync(UserDetailModel model);
}