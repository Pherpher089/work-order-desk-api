using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkOrderDesk.Infrastructure.Persistence;

public sealed class WorkOrderDeskContextFactory : IDesignTimeDbContextFactory<WorkOrderDeskContext>
{
    public WorkOrderDeskContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkOrderDeskContext>();

        optionsBuilder.UseSqlite("Data Source=workorderdesk.db");

        return new WorkOrderDeskContext(optionsBuilder.Options);
    }
}