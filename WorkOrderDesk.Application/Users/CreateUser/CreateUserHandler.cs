using System.Runtime.InteropServices;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.Users.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IUserRepository _usersRepository;

    public CreateUserHandler(IUserRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<CreateUserResult> HandleAsync(
        CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        User user = User.Create(
            firstName: command.FirstName,
            lastName: command.LastName
        );

        await _usersRepository.AddAsync(user, cancellationToken);

        return new CreateUserResult
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}