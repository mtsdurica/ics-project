using ProjectICS.DAL.Entities;

namespace ProjectICS.DAL.Mappers;

public interface IEntityMapper<in TEntity>
    where TEntity : IEntity
{
    void MapToExistingEntity(TEntity entity, TEntity newEntity);
}