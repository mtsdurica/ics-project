using ProjectICS.DAL.Entities;

namespace ProjectICS.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        existingEntity.UserId = newEntity.UserId;
        existingEntity.ProjectId = newEntity.ProjectId;
        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;
        existingEntity.Type = newEntity.Type;
        existingEntity.StartTime = newEntity.StartTime;
        existingEntity.EndTime = newEntity.EndTime;
        existingEntity.ProjectId = newEntity.ProjectId;
    }
}