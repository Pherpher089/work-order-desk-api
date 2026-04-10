using WorkOrderDesk.Application.WorkOrders.GetWorkOrderById;
using WorkOrderDesk.Application.WorkOrders.ListWorkOrders;
using WorkOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Application.Abstractions;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkOrderListItem>> ListAsync(CancellationToken cancellationToken = default);
    Task<WorkOrderDetailsResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<WorkOrder?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
}