using System;
using ProjectICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ProjectICS.DAL.Seeds;

public static class UserSeed
{
    public static readonly UserEntity Freddie = new UserEntity()
    {
        Id = Guid.Parse("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
        Name = "Freddie",
        Surname = "Mercury",
        PictureLink = null,
    };

    public static readonly UserEntity James = new UserEntity()
    {
        Id = Guid.Parse("e9d32406-09fe-4aaf-b6fd-2f48436c1a4a"),
        Name = "James",
        Surname = "Iha",
        PictureLink = "https://media.stubhubstatic.com/stubhub-catalog/d_defaultLogo.jpg/t_f-fs-0fv,q_auto:low,f_auto,c_fill,$w_280_mul_3,$h_180_mul_3/performer/724647/qkkb3sxcpdk0s5dyjrjm"       
    };

    public static readonly UserEntity Billy = new UserEntity()
    {
        Id = Guid.Parse("a7b43393-c7b5-42a1-8dc9-7454a4ef8540"),
        Name = "Billy",
        Surname = "Corgan",
        PictureLink = "https://www.nme.com/wp-content/uploads/2020/09/smashing-pumpkins-getty-images-pedro-gomes-redferns-.jpg"
    };

    public static readonly UserEntity Michael = new UserEntity()
    {
        Id = Guid.Parse("46b9b347-32d6-4367-828a-1b60f3508a83"),
        Name = "Michael",
        Surname = "Jordan",
        PictureLink = "https://upload.wikimedia.org/wikipedia/commons/a/ae/Michael_Jordan_in_2014.jpg"
    };

    static UserSeed()
    {
        Freddie.UserActivities.Add(ActivitySeed.JammingWithFriends);
        Freddie.UserProjects.Add(ProjectSeed.School);
        James.UserActivities.Add(ActivitySeed.Coffee);
        Michael.UserActivities.Add(ActivitySeed.BasketballPractice);
        Billy.UserActivities.Add(ActivitySeed.CookingLesson);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            Freddie with { UserActivities = Array.Empty<ActivityEntity>(), UserProjects = Array.Empty<ProjectEntity>() },
            James with { UserActivities = Array.Empty<ActivityEntity>(), UserProjects = Array.Empty<ProjectEntity>() },
            Billy with { UserActivities = Array.Empty<ActivityEntity>(), UserProjects = Array.Empty<ProjectEntity>() },
            Michael with { UserActivities = Array.Empty<ActivityEntity>(), UserProjects = Array.Empty<ProjectEntity>() }
        );
}