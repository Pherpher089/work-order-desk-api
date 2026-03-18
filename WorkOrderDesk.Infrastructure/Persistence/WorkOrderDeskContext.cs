using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Domain.WorkOrders;
using WorkOrderDesk.Domain.Users;


namespace WorkOrderDesk.Infrastructure.Persistence;

public class WorkOrderDeskContext : DbContext
{
    public WorkOrderDeskContext(DbContextOptions<WorkOrderDeskContext> options) : base(options) { }

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkOrderDeskContext).Assembly);
    }
}
