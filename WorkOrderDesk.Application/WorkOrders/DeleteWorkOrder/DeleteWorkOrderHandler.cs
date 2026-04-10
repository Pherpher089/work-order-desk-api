using WorkOrderDesk.Application.Abstractions;

namespace WorkOrderDesk.Application.WorkOrders.DeleteWorkOrder;

public sealed class DeleteWorkOrderHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;
    public DeleteWorkOrderHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<bool> HandleAsync(
        DeleteWorkOrderCommand command,
        CancellationToken cancellationToken
    )
    {
        var workOrder = await _workOrderRepository.GetEntityByIdAsync(command.Id, cancellationToken);

        if (workOrder == null)
        {
            return false;
        }

        await _workOrderRepository.DeleteAsync(workOrder, cancellationToken);
        await _workOrderRepository.SaveChangesAsync();

        return true;
    }

}