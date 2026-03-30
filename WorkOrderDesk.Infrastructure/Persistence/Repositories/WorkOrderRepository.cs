using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Infrastructure.Persistence.Repositories;

public sealed class WorkOrderRepository : IWorkOrderRepository
{
    private readonly WorkOrderDeskContext _context;

    public WorkOrderRepository(WorkOrderDeskContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default)
    {
        await _context.WorkOrders.AddAsync(workOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}