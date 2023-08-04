using ProjectICS.DAL;
using Microsoft.EntityFrameworkCore;


namespace ProjectICS.Common.Tests.TestingFactories;

public class DbContextLocalDBTestingFactory : IDbContextFactory<ProjectICSDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextLocalDBTestingFactory(string databaseName, bool seedTestingData)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public ProjectICSDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<ProjectICSDbContext> builder = new();
        builder.UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = {_databaseName};MultipleActiveResultSets = True;Integrated Security = True; ");

        return new ProjectICSTestingDbContext(builder.Options, _seedTestingData);
    }
}