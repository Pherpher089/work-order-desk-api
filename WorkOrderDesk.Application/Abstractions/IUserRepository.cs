
using WorkOrderDesk.Application.Users.ListUsers;
using WorkOrderDesk.Domain.Users;
using WorkOrderDesk.Application.Users.GetUserById;
namespace WorkOrderDesk.Application.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserListItem>> ListAsync(CancellationToken cancellationToken = default);

    Task<UserDetailsResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
