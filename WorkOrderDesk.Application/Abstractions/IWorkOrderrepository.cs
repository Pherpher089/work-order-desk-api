using WorkOrderDesk.Application.WorkOrders.ListWorkOrders;
using WorkOrderDesk.Domain.WorkOrders;
namespace WorkOrderDesk.Application.Abstractions;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkOrderListItem>> ListAsync(CancellationToken cancellationToken = default);
}