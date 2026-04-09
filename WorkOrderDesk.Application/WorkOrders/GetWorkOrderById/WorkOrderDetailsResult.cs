namespace WorkOrderDesk.Application.WorkOrders.GetWorkOrderById;

public sealed class WorkOrderDetailsResult
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public Guid? AssigneeId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public DateTime? CompletedAtUtc { get; init; }
    public DateTime? ArchivedAtUtc { get; init; }
}