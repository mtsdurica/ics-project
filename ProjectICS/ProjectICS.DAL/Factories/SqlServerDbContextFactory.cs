using Microsoft.EntityFrameworkCore;

namespace ProjectICS.DAL.Factories;

public class SqlServerDbContextFactory : IDbContextFactory<ProjectICSDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<ProjectICSDbContext> _contextOptionsBuilder = new();

    public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        _contextOptionsBuilder.UseSqlServer(connectionString);

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.LogTo(System.Console.WriteLine);
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
    }

    public ProjectICSDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
}
