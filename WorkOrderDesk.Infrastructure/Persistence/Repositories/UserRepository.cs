
using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Application.Users.GetUserById;
using WorkOrderDesk.Application.Users.ListUsers;
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

    public async Task<IReadOnlyList<UserListItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Select(x => new UserListItem
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDetailsResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = new UserId(id);

        return await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new UserDetailsResult
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

}