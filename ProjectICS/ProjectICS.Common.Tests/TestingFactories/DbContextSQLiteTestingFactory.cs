using ProjectICS.DAL;
using Microsoft.EntityFrameworkCore;


namespace ProjectICS.Common.Tests.TestingFactories;

public class DbContextSQLiteTestingFactory : IDbContextFactory<ProjectICSDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextSQLiteTestingFactory(string databaseName, bool seedTestingData)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public ProjectICSDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<ProjectICSDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

        //builder.EnableSensitiveDataLogging();
        //builder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        

        return new ProjectICSTestingDbContext(builder.Options, _seedTestingData);
    }
}