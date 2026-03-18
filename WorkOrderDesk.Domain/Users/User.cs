namespace WorkOrderDesk.Domain.Users;

public sealed class User
{
    public UserId Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public User(UserId id, string firstName, string lastName)
    {
        if (id.Value == Guid.Empty) throw new ArgumentException("UserId cannot be empty", nameof(Id));
        FirstName = NormalizeName(firstName, nameof(firstName));
        LastName = NormalizeName(lastName, nameof(lastName));
        Id = id;
    }

    public static User Create(string firstName, string lastName) => new(UserId.New(), firstName, lastName);

    public void Rename(string firstName, string lastName)
    {
        FirstName = NormalizeName(firstName, nameof(firstName));
        LastName = NormalizeName(lastName, nameof(lastName));
    }
    private static string NormalizeName(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name cannont be empty", paramName);
        string trimmed = value.Trim();
        if (trimmed.Length is < 1 or > 100) throw new ArgumentException("Name must be 1-100 characters", paramName);
        return trimmed;
    }
}