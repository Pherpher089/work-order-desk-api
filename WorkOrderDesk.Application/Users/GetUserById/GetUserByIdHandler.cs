using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.Users.GetUserById;

public sealed class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDetailsResult?> HandleAsync(
        GetUserByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        return await _userRepository.GetByIdAsync(query.Id, cancellationToken);
    }
}