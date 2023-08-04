using ProjectICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace ProjectICS.Common.Tests.TestingSeeds;

public static class UserEntitySeeds
{
    public static readonly UserEntity EmptyUser = new UserEntity()
    {
        Id = default,
        Name = default!,
        Surname = default!,
        PictureLink = default,
    };

    public static readonly UserEntity SeededUser = new UserEntity()
    {
        Id = Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
        Name = "Freddie",
        Surname = "Mercury",
        PictureLink = "https://cdn.britannica.com/38/200938-050-E22981D1/Freddie-Mercury-Live-Aid-Queen-Wembley-Stadium-July-13-1985.jpg",
    };

    public static readonly UserEntity SeededUser2 = new UserEntity()
    {
        Id = Guid.Parse(input: "06a8b2cf-ea03-4095-a2e4-aa0281fe9c75"),
        Name = "Stephen",
        Surname = "Curry",
        PictureLink = "https://a.espncdn.com/combiner/i?img=/i/headshots/nba/players/full/3975.png&w=350&h=254",
    };


    public static readonly UserEntity SeededUserUpdate = SeededUser with { Id = Guid.Parse("820c6165-2ba7-4a17-9524-c9f7b2e4f20f") } ;

    public static readonly UserEntity SeededUserToAddActivity = SeededUser with { Id = Guid.Parse("820a6165-2ba7-4a17-9624-c9f7b2e4f20f") };

    public static readonly UserEntity SeededUserDelete = SeededUser with { Id = Guid.Parse("ee46348c-6697-4431-89e0-a8b9668eef3e") } ;

    

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            SeededUser,
            SeededUser2,
            SeededUserUpdate,
            SeededUserDelete
        );
    }
}