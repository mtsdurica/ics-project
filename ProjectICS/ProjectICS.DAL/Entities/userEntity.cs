namespace ProjectICS.DAL.Entities
{
   
    public record UserEntity : IEntity
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? PictureLink { get; set; }
        public ICollection<ProjectEntity> UserProjects { get; init; } = new List<ProjectEntity>();
        public ICollection<ActivityEntity> UserActivities { get; init; } = new List<ActivityEntity>();
        public required Guid Id { get; set; }
    }
}
