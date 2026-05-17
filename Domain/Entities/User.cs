using Domain.Interfaces;

namespace Domain.Entities;

public class User : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public int? LevelId { get; private set; }
    public int RoleId { get; private set; }

    public Level? Level { get; private set; }
    public Role? Role { get; private set; }

    private User(string name, string email, string passwordHash,
        int roleId, DateTime createdAt, DateTime? updatedAt)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        LevelId = null;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static User New(string name, string email, string passwordHash, int roleId)
        => new User(name, email, passwordHash, roleId, DateTime.UtcNow, null);

    public void UpdateProfile(string name, string email)
    {
        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeRole(int roleId)
    {
        RoleId = roleId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetLevel(int levelId)
    {
        LevelId = levelId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}