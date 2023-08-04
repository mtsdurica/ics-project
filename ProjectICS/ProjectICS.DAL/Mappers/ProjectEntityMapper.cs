using ProjectICS.DAL.Entities;

namespace ProjectICS.DAL.Mappers;

public class ProjectEntityMapper : IEntityMapper<ProjectEntity>
{
    public void MapToExistingEntity(ProjectEntity existingEntity, ProjectEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        existingEntity.UserId = newEntity.UserId;
        existingEntity.Name = newEntity.Name;
    }
}