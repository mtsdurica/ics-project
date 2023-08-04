using System.Runtime.CompilerServices;
using ProjectICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectICS.DAL.Seeds;

namespace ProjectICS.Common.Tests.TestingSeeds;

public static  class ProjectEntitySeeds
{
    public static readonly ProjectEntity EmptyProject = new()
    {
        Id = default,
        UserId = default,
        Name = default!
    };

    public static readonly ProjectEntity SchoolProj = new()
    {
        Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        UserId = UserEntitySeeds.SeededUser.Id,
        Name = "School"
    };

    public static readonly ProjectEntity SeededProjectUpdate = SchoolProj with { Id = Guid.Parse("860c6165-2ba7-4a17-0024-c9f7b2e4f20f") };
    
    public static readonly ProjectEntity SeededProjectDelete = SchoolProj with { Id = Guid.Parse("ee46348c-6697-4431-89e0-a8b9333eef3e") };



    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectEntity>().HasData(
            SchoolProj,
            SeededProjectUpdate,
            SeededProjectDelete
        );
    }
}