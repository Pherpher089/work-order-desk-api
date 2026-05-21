
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly WorkOrderDeskContext _context;

    public UserRepository(WorkOrderDeskContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}