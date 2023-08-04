namespace ProjectICS.App.Messages;

public record ProjectEditMessage
{
    public required Guid ProjectId {get; set; }
}