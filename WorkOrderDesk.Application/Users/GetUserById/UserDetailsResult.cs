using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.Users.GetUserById;

public sealed class UserDetailsResult
{
    public UserId Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}