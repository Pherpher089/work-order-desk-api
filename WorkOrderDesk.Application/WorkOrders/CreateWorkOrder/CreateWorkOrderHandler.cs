using System.Security.Cryptography.X509Certificates;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Domain.WorkOrders;

namespace WorkOrderDesk.Application.WorkOrders.CreateWorkOrder;

public sealed class CreateWorkOrderHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public CreateWorkOrderHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<CreateWorkOrderResult> HandleAsync(
        CreateWorkOrderCommand command,
        CancellationToken cancellationToken = default
    )
    {
        WorkOrder workOrder = WorkOrder.Create(
            title: command.Title,
            description: command.Description,
            priority: command.Priority
        );

        await _workOrderRepository.AddAsync(workOrder, cancellationToken);

        return new CreateWorkOrderResult
        {
            Id = workOrder.Id,
            Title = workOrder.Title,
            Description = workOrder.Description,
            Priority = workOrder.Priority.ToString(),
            Status = workOrder.Status.ToString(),
            CreatedAtUtc = workOrder.CreatedAtUtc
        };
    }
}
