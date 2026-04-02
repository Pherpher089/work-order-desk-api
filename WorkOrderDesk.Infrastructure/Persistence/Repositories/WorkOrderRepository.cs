using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Application.WorkOrders.ListWorkOrders;
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

    public async Task<IReadOnlyList<WorkOrderListItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new WorkOrderListItem
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Priority = x.Priority.ToString(),
                Status = x.Status.ToString(),
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }
}