namespace WorkOrderDesk.Domain.Users;

/// <summary>
/// Simple typed ID to avoid passing arround raw Guids everywhere. 
/// </summary>
public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();

}