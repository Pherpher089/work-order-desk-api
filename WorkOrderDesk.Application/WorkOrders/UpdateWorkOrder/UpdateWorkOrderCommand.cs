using WorkOrderDesk.Domain.WorkOrders;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.WorkOrders.UpdateWorkOrder;

public sealed class UpdateWorkOrderCommand
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public WorkOrderPriority Priority { get; init; }
    public WorkOrderStatus Status { get; init; }
    public Guid? AssigneeId { get; init; }
}