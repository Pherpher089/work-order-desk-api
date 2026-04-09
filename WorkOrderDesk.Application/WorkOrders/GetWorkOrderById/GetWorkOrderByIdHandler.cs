using WorkOrderDesk.Application.WorkOrders.GetWorkOrderById;
using WorkOrderDesk.Application.Abstractions;

namespace WorkOrderDesk.Application.WorkOrders.GetWorkOrderById;

public sealed class GetWorkOrderByIdHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrderByIdHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<WorkOrderDetailsResult?> HandleAsync(
        GetWorkOrderByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        return await _workOrderRepository.GetByIdAsync(query.Id, cancellationToken);
    }
}