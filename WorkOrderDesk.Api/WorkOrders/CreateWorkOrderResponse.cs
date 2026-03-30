namespace workOrderDesk.Api.WorkOrders;

public sealed class CreateWorkOrderResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
}