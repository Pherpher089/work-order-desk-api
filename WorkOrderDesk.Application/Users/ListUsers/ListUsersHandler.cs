using WorkOrderDesk.Application.Abstractions;

namespace WorkOrderDesk.Application.Users.ListUsers;

public sealed class ListUsersHandler
{
    private readonly IUserRepository _userRepository;

    public ListUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserListItem>> HandleAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _userRepository.ListAsync(cancellationToken);
    }
}

