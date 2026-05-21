
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Application.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
