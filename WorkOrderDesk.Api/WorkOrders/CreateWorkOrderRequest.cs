using WorkOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Api.WorkOrders;

public sealed class CreateWorkOrderRequest
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public WorkOrderPriority Priority { get; init; } = WorkOrderPriority.Medium;
}