namespace ProjectICS.DAL.Entities;


public record ProjectEntity : IEntity
    {
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<UserEntity> ProjectUsers { get; init; } = new List<UserEntity>();
        public ICollection<ActivityEntity> ProjectActivities { get; set; } = new List<ActivityEntity>();
        public required Guid Id { get; set; }
    }