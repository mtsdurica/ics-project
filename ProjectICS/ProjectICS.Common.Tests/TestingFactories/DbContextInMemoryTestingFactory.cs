using Microsoft.EntityFrameworkCore;
using ProjectICS.DAL;
        


namespace ProjectICS.Common.Tests.TestingFactories;

public class DbContextInMemoryTestingFactory : IDbContextFactory<ProjectICSDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextInMemoryTestingFactory(string databaseName, bool seedTestingData)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public ProjectICSDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<ProjectICSDbContext> contextOptionsBuilder = new();
        contextOptionsBuilder.UseInMemoryDatabase(_databaseName);

        return new ProjectICSTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
    }
}