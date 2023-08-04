using System;
using System.Threading.Tasks;
using ProjectICS.DAL.Entities;
using ProjectICS.DAL.Mappers;
using ProjectICS.DAL.Repositories;


namespace ProjectICS.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    Task CommitAsync();
}