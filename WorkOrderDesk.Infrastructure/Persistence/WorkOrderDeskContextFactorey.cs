using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkOrderDesk.Infrastructure.Persistence;

public sealed class WorkOrderDeskContextFactory : IDesignTimeDbContextFactory<WorkOrderDeskContext>
{
    public WorkOrderDeskContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkOrderDeskContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=workorderdesk;Username=wod_user;Password=password"
        );
        return new WorkOrderDeskContext(optionsBuilder.Options);
    }
}