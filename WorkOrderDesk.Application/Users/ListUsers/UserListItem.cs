using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.Users.ListUsers;

public sealed class UserListItem
{
    public UserId Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}