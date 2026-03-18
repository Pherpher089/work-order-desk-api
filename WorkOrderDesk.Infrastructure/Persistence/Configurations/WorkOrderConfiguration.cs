using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkOrderDesk.Domain.WorkOrders;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("WorkOrders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        builder.Property(x => x.CompletedAtUtc);

        builder.Property(x => x.ArchivedAtUtc);

        builder.Property(x => x.AssigneeId)
            .HasConversion(
                assigneeId => assigneeId.HasValue ? assigneeId.Value.Value : (Guid?)null,
                value => value.HasValue ? new UserId(value.Value) : null);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Priority);
        builder.HasIndex(x => x.AssigneeId);
    }
}