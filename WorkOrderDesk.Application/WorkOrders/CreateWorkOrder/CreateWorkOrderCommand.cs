using workOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Application.WorkOrders.CreateWorkOrder;

public sealed class CreateWorkOrderCommand
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public WorkOrderPriority Priority { get; init; } = WorkOrderPriority.Medium;
}