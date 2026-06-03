using WorkOrderDesk.Domain.Users;

namespace WorkOrderdeks.Api.Users;

public sealed class UserListItemResponse
{
    public UserId Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}