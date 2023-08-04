using Microsoft.EntityFrameworkCore;
using ProjectICS.BL.Mappers;
using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Models;
using ProjectICS.DAL.Entities;
using ProjectICS.DAL.Mappers;
using ProjectICS.DAL.Repositories;
using ProjectICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectICS.BL.Facades;

public class ActivityFacade : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade 
{
    private readonly IActivityModelMapper _modelMapper;
    private readonly ActivityEntityMapper _entityMapper;
    public ActivityFacade(IUnitOfWorkFactory unitOfWorkFactory, IActivityModelMapper modelMapper, ActivityEntityMapper entityMapper) : base(unitOfWorkFactory, modelMapper)
    {
        _modelMapper = modelMapper;
        _entityMapper = entityMapper;
    }
    public async Task AddActivityToProjectAsync(ActivityListModel model, Guid userId, Guid projectId)
    {
        ActivityEntity searchedEntity = _modelMapper.MapToEntity(model, userId);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository =
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        if (await repository.ExistsAsync(searchedEntity))
        {
            searchedEntity = await repository.UpdateAsync(searchedEntity);
            searchedEntity.ProjectId = projectId;
            await uow.CommitAsync();
        }
    }

    public async Task UpdateAsync(ActivityDetailModel model)
    {
        ActivityEntity searchedEntity = _modelMapper.MapToEntity(model, model.UserId);
        ActivityEntity updatedEntity = _modelMapper.MapToEntity(model, model.UserId);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository =
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var activitiesToCheck = await GetAllByIdAsync(model.UserId);

        searchedEntity = await repository.UpdateAsync(searchedEntity);

        bool conflict = true;

        if (searchedEntity.StartTime == updatedEntity.StartTime && searchedEntity.EndTime == updatedEntity.EndTime)
        {
            conflict = false;
        }
        else { conflict = CheckConflicts(updatedEntity, activitiesToCheck); }

        if (conflict is not true)
        {
            if (await repository.ExistsAsync(searchedEntity))
            {
                searchedEntity = await repository.UpdateAsync(searchedEntity);
                _entityMapper.MapToExistingEntity(searchedEntity, updatedEntity);
                await uow.CommitAsync();
            }
        }
        else
        {
            throw new InvalidOperationException("Dates in conflict");
        }
        
    }

    public async Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid userId)
    {
        ActivityEntity entity = _modelMapper.MapToEntity(model, userId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository =
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var activitiesToCheck = await GetAllByIdAsync(userId);

        bool conflict = CheckConflicts(entity, activitiesToCheck);

        if (conflict is not true)
        {
            await repository.InsertAsync(entity);

            await uow.CommitAsync();

            return ModelMapper.MapToDetailModel(entity);
        }
        else
        {
            throw new InvalidOperationException("Dates in conflict");
        }
    }

    public bool CheckConflicts(ActivityEntity entity, IEnumerable<ActivityEntity> activitiesToCheck)
    {
        bool conflict = true;

        if (entity.StartTime >= entity.EndTime)
        {
            return conflict;
        }

        if (activitiesToCheck.Count() == 0)
        {
            return false;
        }

        foreach (var activity in activitiesToCheck)
        {
            if (entity.Id != activity.Id)
            {
                if (entity.EndTime < activity.StartTime)
                {
                    conflict = false;
                }
                else if (entity.StartTime > activity.EndTime)
                {
                    conflict = false;
                }
                else
                {
                    conflict = true;
                    break;
                }
            }
           
        }
        return conflict;
    }

    public async Task<IEnumerable<ActivityEntity>> GetByProjectIdAsync(Guid projectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.ProjectId == projectId)
            .ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<ActivityEntity>> GetAllByIdAsync(Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return entities;
    }

    public async Task <IEnumerable<ActivityListModel>> GetFilteredActivitiesAsync(Guid userId, string firstSelected, string secondSelected)
    {
        DateTime filter;
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities;
        List<ActivityListModel> returnActivities;
        if (firstSelected is null)
        {
            switch (secondSelected)
            {
                case "Ascending":
                    entities = await uow
                        .GetRepository<ActivityEntity, ActivityEntityMapper>()
                        .Get()
                        .Where(e => e.UserId == userId)
                        .OrderBy(e => e.StartTime)
                        .ToListAsync();
                    break;
                case "Descending":
                    entities = await uow
                        .GetRepository<ActivityEntity, ActivityEntityMapper>()
                        .Get()
                        .Where(e => e.UserId == userId)
                        .OrderByDescending(e => e.StartTime)
                        .ToListAsync();
                    break;
                default:
                    return null;
            }
        }
        else
        {
            switch (firstSelected)
            {
                case "This day":
                    filter = DateTime.Today;
                    if (secondSelected == "Ascending")
                    {
                        entities = await uow
                                .GetRepository<ActivityEntity, ActivityEntityMapper>()
                                .Get()
                                .Where(e => e.UserId == userId)
                                .Where(e => e.StartTime.Day == filter.Day)
                                .Where(e => e.StartTime.Month == filter.Month)
                                .Where(e => e.StartTime.Year == filter.Year)
                                .OrderBy(e => e.StartTime)
                                .ToListAsync();
                    }
                    else
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Day == filter.Day)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderByDescending(e => e.StartTime)
                            .ToListAsync();
                    }
                    break;
                case "Last week":
                    filter = DateTime.Today;
                    if (secondSelected == "Ascending")
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Day >= filter.Day - 7 && e.StartTime.Day <= filter.Day)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderBy(e => e.StartTime)
                            .ToListAsync();
                    }
                    else
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Day >= filter.Day - 7 && e.StartTime.Day <= filter.Day)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderByDescending(e => e.StartTime)
                            .ToListAsync();
                    }
                    break;
                case "This week":
                    filter = DateTime.Today;
                    if (secondSelected == "Ascending")
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Day <= filter.Day + 7 && e.StartTime.Day >= filter.Day)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderBy(e => e.StartTime)
                            .ToListAsync();
                    }
                    else
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Day <= filter.Day + 7 && e.StartTime.Day >= filter.Day)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderByDescending(e => e.StartTime)
                            .ToListAsync();
                    }
                    break;
                case "This month":
                    filter = DateTime.Today;
                    if (secondSelected == "Ascending")
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderBy(e => e.StartTime)
                            .ToListAsync();
                    }
                    else
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Month == filter.Month)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderByDescending(e => e.StartTime)
                            .ToListAsync();
                    }
                    break;
                case "Last month":
                    filter = DateTime.Today;
                    if (secondSelected == "Ascending")
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Month == filter.Month - 1)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderBy(e => e.StartTime)
                            .ToListAsync();
                    }
                    else
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Month == filter.Month - 1)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderByDescending(e => e.StartTime)
                            .ToListAsync();
                    }
                    break;
                case "This year":
                    filter = DateTime.Today;
                    if (secondSelected == "Ascending")
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderBy(e => e.StartTime)
                            .ToListAsync();
                    }
                    else
                    {
                        entities = await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.UserId == userId)
                            .Where(e => e.StartTime.Year == filter.Year)
                            .OrderByDescending(e => e.StartTime)
                            .ToListAsync();
                    }
                    break;
                default:
                    return null;
            }
        }
        returnActivities = ModelMapper.MapToListModel(entities).ToList();
        return returnActivities;
    }
}