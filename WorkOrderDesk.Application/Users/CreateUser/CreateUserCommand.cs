using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.Users.CreateUser;

public sealed class CreateUserCommand
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}