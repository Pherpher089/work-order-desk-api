using System.Dynamic;
using workOrderDesk.Domain.WorkOrders;
using WorkOrderDesk.Domain.Users;

namespace WorkOrderDesk.Domain.WorkOrders;

public sealed class WorkOrder
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public string? Description { get; private set; }

    public WorkOrderPriority Priority { get; private set; } = WorkOrderPriority.Medium;
    public WorkOrderStatus Status { get; private set; } = WorkOrderStatus.Open;

    public UserId? AssigneeId { get; private set; }

    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }

    public DateTime? CompletedAtUtc { get; private set; }
    public DateTime? ArchivedAtUtc { get; private set; }

    private WorkOrder(
        Guid id,
        string title,
        string? description,
        WorkOrderPriority priority,
        WorkOrderStatus status,
        UserId? assigneeId,
        DateTime createdAtUtc,
        DateTime updatedAtUtc,
        DateTime? completedAtUtc,
        DateTime? archivedAtUtc
    )
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
        Status = status;
        AssigneeId = assigneeId;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
        CompletedAtUtc = completedAtUtc;
        ArchivedAtUtc = archivedAtUtc;
    }

    public static WorkOrder Create(string title, string? description = null, WorkOrderPriority priority = WorkOrderPriority.Medium)
    {
        DateTime now = DateTime.UtcNow;
        string normalizedTitle = NormalizeTitle(title);
        string? normalizedDescription = NormalizeDescription(description);

        return new WorkOrder(
            id: Guid.NewGuid(),
            title: normalizedTitle,
            description: normalizedDescription,
            priority: priority,
            status: WorkOrderStatus.Open,
            assigneeId: null,
            createdAtUtc: now,
            updatedAtUtc: now,
            completedAtUtc: null,
            archivedAtUtc: null
        );

    }

    public void SetTitle(string title)
    {
        string normalized = NormalizeTitle(title);
        if (normalized == Title) return;

        Title = normalized;
        Touch();
    }

    public void SetDescription(string? description)
    {
        string? normalized = NormalizeDescription(description);
        if (normalized == description) return;

        Description = normalized;
        Touch();
    }

    public void SetPriority(WorkOrderPriority priority)
    {
        if (priority == Priority) return;

        Priority = priority;
        Touch();
    }

    public void AssignTo(UserId userId)
    {
        if (userId.Value == Guid.Empty) throw new ArgumentException("AssigneeId cannot be empty.", nameof(userId));
        if (AssigneeId == userId) return;

        AssigneeId = userId;

        // If currently invalid because we're InProgrees/Complete, assignment fixes it.
        Touch();
    }

    public void Unassign()
    {
        if (AssigneeId is null) return;

        // Domain rule: InProgress and complete require an assingee
        if (Status is WorkOrderStatus.InProgress or WorkOrderStatus.Complete)
            throw new InvalidOperationException("Cannot unassign a work order that is in progress or complete");

        AssigneeId = null;
        Touch();
    }

    public void MoveTo(WorkOrderStatus newStatus)
    {
        if (newStatus == Status) return;

        if (Status == WorkOrderStatus.Archived)
            throw new InvalidOperationException("Archived work orders cannot change status.");

        // Archive can happen from any non-archived status.
        if (newStatus == WorkOrderStatus.Archived)
        {
            Archive();
            return;
        }

        // Linear movement only (adjacent steps)
        if (!IsLinearAdjacentMove(Status, newStatus))
            throw new InvalidOperationException($"Invalid status transition: {Status}");


        // enforce assignee requirement on entering InProgress / Complete
        if (newStatus is WorkOrderStatus.InProgress or WorkOrderStatus.Complete)
        {
            if (AssigneeId is null)
                throw new InvalidOperationException($"Cannot move to {newStatus} without an assignee.");
        }

        // Hanlde CompletedAtUtc
        if (newStatus == WorkOrderStatus.Complete)
            CompletedAtUtc = DateTime.UtcNow;
        else
            CompletedAtUtc = null;

        Status = newStatus;
        Touch();
    }

    public void Archive()
    {
        if (Status == WorkOrderStatus.Archived)
            return;

        // Once archived, we treat it as terminal.
        Status = WorkOrderStatus.Archived;
        ArchivedAtUtc = DateTime.UtcNow;

        Touch();
    }

    private static bool IsLinearAdjacentMove(WorkOrderStatus from, WorkOrderStatus to)
    {
        // Backlog ↔ Open ↔ InProgress ↔ Complete (adjacent only)
        // Archive handle elsewhere
        int diff = Math.Abs((int)to - (int)from);
        return diff == 1 && from != WorkOrderStatus.Archived && to != WorkOrderStatus.Archived;
    }

    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.", nameof(title));
        string trimmed = title.Trim();
        if (trimmed.Length is < 3 or > 100) throw new ArgumentException("Title must be 3-100 characters.", nameof(title));
        return trimmed;
    }
    private static string? NormalizeDescription(string? description)
    {
        if (description == null) return null;
        string trimmed = description.Trim();
        if (trimmed.Length == 0) return null;
        if (trimmed.Length > 2000) throw new ArgumentException("Description must be  <= 2000 character.", nameof(description));
        return trimmed;
    }
}