using ProjectICS.Common.Tests.TestingSeeds;
using ProjectICS.DAL;
using Microsoft.EntityFrameworkCore;

namespace ProjectICS.Common.Tests;

public class ProjectICSTestingDbContext : ProjectICSDbContext
{
    private readonly bool _seedTestingData;

    public ProjectICSTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedDemoData: false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            ActivityEntitySeeds.Seed(modelBuilder);
            ProjectEntitySeeds.Seed(modelBuilder);
            UserEntitySeeds.Seed(modelBuilder);
        }
    }
}