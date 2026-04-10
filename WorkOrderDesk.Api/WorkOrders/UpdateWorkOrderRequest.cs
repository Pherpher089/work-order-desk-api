using WorkOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Api.WorkOrders;

public sealed class UpdateWorkOrderRequest
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public WorkOrderPriority Priority { get; init; }
    public WorkOrderStatus Status { get; init; }
    public Guid? AssigneeId { get; init; }
}