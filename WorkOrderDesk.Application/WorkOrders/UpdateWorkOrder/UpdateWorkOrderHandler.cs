using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Application.WorkOrders.UpdateWorkOrder;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.WorkOrders.UpdateWorkOrder;

public sealed class UpdateWorkOrderHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public UpdateWorkOrderHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<UpdateWorkOrderResult?> HandleAsync(
        UpdateWorkOrderCommand command,
        CancellationToken cancellationToken
    )
    {
        var workOrder = await _workOrderRepository.GetEntityByIdAsync(command.Id, cancellationToken);

        if (workOrder is null)
        {
            return null;
        }

        workOrder.SetTitle(command.Title);
        workOrder.SetDescription(command.Description);
        workOrder.SetPriority(command.Priority);

        if (command.AssigneeId.HasValue)
        {
            workOrder.AssignTo(new UserId(command.AssigneeId.Value));
        }
        else
        {
            workOrder.Unassign();
        }

        workOrder.MoveTo(command.Status);

        await _workOrderRepository.SaveChangesAsync(cancellationToken);

        return new UpdateWorkOrderResult
        {
            Id = workOrder.Id,
            Title = workOrder.Title,
            Description = workOrder.Description,
            Priority = workOrder.Priority.ToString(),
            Status = workOrder.Status.ToString(),
            AssigneeId = workOrder.AssigneeId?.Value,
            CreatedAtUtc = workOrder.CreatedAtUtc,
            UpdatedAtUtc = workOrder.UpdatedAtUtc,
            CompletedAtUtc = workOrder.CompletedAtUtc,
            ArchivedAtUtc = workOrder.ArchivedAtUtc
        };
    }
}