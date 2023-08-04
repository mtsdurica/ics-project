using Microsoft.EntityFrameworkCore;
using ProjectICS.DAL.Entities;
using ProjectICS.DAL.Seeds;

namespace ProjectICS.DAL;

    public class ProjectICSDbContext : DbContext
    {

    private readonly bool _seedDemoData;
    public ProjectICSDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
        : base(contextOptions) => _seedDemoData = seedDemoData;

        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();

        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();

        public DbSet<UserEntity> Users => Set<UserEntity>();

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectEntity>()
                .HasMany(i => i.ProjectActivities)
                .WithOne(i => i.Project)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.UserActivities)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.UserProjects)
                .WithOne(i => i.User);
            
            if (_seedDemoData)
            {
                UserSeed.Seed(modelBuilder);
                ProjectSeed.Seed(modelBuilder);
                ActivitySeed.Seed(modelBuilder);
            }
        }             
        

    }

   

