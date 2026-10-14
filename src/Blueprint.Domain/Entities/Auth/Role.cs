using Blueprint.Domain.Common.Entities;

namespace Blueprint.Domain.Entities.Auth;

public class Role : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() : base() { }

    public Role(string name, string? description = null) : base()
    {
        Name = name.Trim();
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name.Trim();
        Description = description;
    }
}