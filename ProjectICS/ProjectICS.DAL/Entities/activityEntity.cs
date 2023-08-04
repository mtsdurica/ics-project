namespace ProjectICS.DAL.Entities
{

    public record ActivityEntity : IEntity
    {
        public required Guid UserId { get; set; }
        public Guid? ProjectId { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ProjectEntity? Project { get; init; }
        public UserEntity? User { get; init; }
        public required Guid Id { get; set; }
    }
}
