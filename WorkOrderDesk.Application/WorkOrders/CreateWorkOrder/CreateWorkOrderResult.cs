namespace WorkOrderDesk.Application.WorkOrders.CreateWorkOrder;

public sealed class CreateWorkOrderResult
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreateAtUtc { get; init; }
}