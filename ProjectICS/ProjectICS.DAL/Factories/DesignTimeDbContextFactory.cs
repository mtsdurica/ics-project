using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectICS.DAL.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectICSDbContext>
    {
        private readonly DbContextSqLiteFactory _dbContextSqLiteFactory;
        private const string ConnectionString = $"Data Source=ActivityTracker.db;Cache=Shared";

        public DesignTimeDbContextFactory()
        {
            _dbContextSqLiteFactory = new DbContextSqLiteFactory(ConnectionString);
        }

        public ProjectICSDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
    }
}
