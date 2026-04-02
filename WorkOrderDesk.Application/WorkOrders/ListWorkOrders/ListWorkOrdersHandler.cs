using WorkOrderDesk.Application.Abstractions;

namespace WorkOrderDesk.Application.WorkOrders.ListWorkOrders;

public sealed class ListWorkOrdersHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public ListWorkOrdersHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<IReadOnlyList<WorkOrderListItem>> HandleAsync(
        ListWorkOrdersQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await _workOrderRepository.ListAsync(cancellationToken);
    }
}