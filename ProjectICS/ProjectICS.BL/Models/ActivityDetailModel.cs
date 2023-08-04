using System;

namespace ProjectICS.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required Guid UserId { get; set; }
    public  Guid? ProjectId { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public static ActivityDetailModel Empty
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
            StartTime = DateTime.Today,
            EndTime = DateTime.Today
        };
}