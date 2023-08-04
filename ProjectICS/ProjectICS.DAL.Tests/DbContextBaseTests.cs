using System;
using System.Threading.Tasks;
using ProjectICS.DAL.Factories;
using ProjectICS.Common.Tests;
using ProjectICS.Common.Tests.TestingFactories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;


namespace ProjectICS.DAL.Tests
{
    public class DbContextTestsBase : IAsyncLifetime
    {
        protected DbContextTestsBase(ITestOutputHelper output)
        {
            DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedTestingData: true);

            ProjectICSDbContextSUT = DbContextFactory.CreateDbContext();

        }

        protected IDbContextFactory<ProjectICSDbContext> DbContextFactory { get; }
        protected ProjectICSDbContext ProjectICSDbContextSUT { get; }


        public async Task InitializeAsync()
        {
            await ProjectICSDbContextSUT.Database.EnsureDeletedAsync();
            await ProjectICSDbContextSUT.Database.EnsureCreatedAsync();

        }

        public async Task DisposeAsync()
        {
            await ProjectICSDbContextSUT.Database.EnsureDeletedAsync();
            await ProjectICSDbContextSUT.DisposeAsync();
        }
    }
}