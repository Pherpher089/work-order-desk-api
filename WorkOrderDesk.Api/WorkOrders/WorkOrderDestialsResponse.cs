namespace WorkOrderDesk.Api.WorkOrders;

public sealed class WorkOrderDetailsResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public Guid? AssigneeId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public DateTime? CompltedAtUtc { get; init; }
    public DateTime? ArchivedAtUtc { get; init; }
}