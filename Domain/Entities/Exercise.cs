using Domain.Interfaces;

namespace Domain.Entities;

public class Exercise : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public int LevelId { get; private set; }
    public int TypeId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }

    public Level? Level { get; private set; }
    public ExerciseType? Type { get; private set; }
    public ICollection<Question>? Questions { get; private set; } = [];

    private Exercise(int levelId, int typeId, string title, string content,
        DateTime createdAt, DateTime? updatedAt)
    {
        LevelId = levelId;
        TypeId = typeId;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Exercise New(int levelId, int typeId, string title, string content)
        => new Exercise(levelId, typeId, title, content, DateTime.UtcNow, null);

    public void UpdateDetails(int levelId, int typeId, string title, string content)
    {
        LevelId = levelId;
        TypeId = typeId;
        Title = title;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
}