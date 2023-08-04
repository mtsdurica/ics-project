using Microsoft.EntityFrameworkCore;

namespace ProjectICS.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<ProjectICSDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<ProjectICSDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}