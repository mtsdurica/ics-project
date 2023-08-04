using System;
using System.Threading.Tasks;
using ProjectICS.BL.Mappers;
using ProjectICS.Common.Tests;
using ProjectICS.Common.Tests.TestingFactories;
using ProjectICS.DAL;
using ProjectICS.DAL.Mappers;
using ProjectICS.DAL.UnitOfWork;
using Xunit;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjectICS.BL.Mappers.Interfaces;
using ProjectICS.BL.Facades;

namespace ProjectICS.BL.Tests
{
    public class FacadeTestsBase : IAsyncLifetime
    { 
        protected IDbContextFactory<ProjectICSDbContext> DbContextFactory { get; }

        protected ActivityEntityMapper ActivityEntityMapper { get; }
        protected UserEntityMapper UserEntityMapper { get; }
        protected ProjectEntityMapper ProjectEntityMapper { get; }
        protected IProjectModelMapper ProjectModelMapper { get; }
        protected IActivityModelMapper ActivityModelMapper { get; }
        protected IUserModelMapper UserModelMapper { get; }
        protected IActivityFacade ActivityFacade { get; }
        protected IUserFacade UserFacade { get; }
        protected UnitOfWorkFactory UnitOfWorkFactory { get; }

        protected FacadeTestsBase(ITestOutputHelper output)
        {
            DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedTestingData: true);

            ActivityEntityMapper = new ActivityEntityMapper();
            UserEntityMapper = new UserEntityMapper();
            ProjectEntityMapper = new ProjectEntityMapper();

            ActivityModelMapper = new ActivityModelMapper();
            ProjectModelMapper = new ProjectModelMapper(ActivityModelMapper);
            UserModelMapper = new UserModelMapper(ProjectModelMapper, ActivityModelMapper);

            UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);

            ActivityFacade = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, ActivityEntityMapper);

            UserFacade = new UserFacade(UnitOfWorkFactory, UserModelMapper, UserEntityMapper);
        }


        public async Task InitializeAsync()
        {
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            await dbx.Database.EnsureDeletedAsync();
            await dbx.Database.EnsureCreatedAsync();

        }

        public async Task DisposeAsync()
        {
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            await dbx.Database.EnsureDeletedAsync();
        }
    }
}