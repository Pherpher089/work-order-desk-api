using WorkOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Application.Abstractions;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
}