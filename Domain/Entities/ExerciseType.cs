using Domain.Interfaces;

namespace Domain.Entities;

public class ExerciseType : IEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    private ExerciseType(string name)
    {
        Name = name;
    }

    public static ExerciseType New(string name)
        => new ExerciseType(name);

    public void UpdateDetails(string name)
    {
        Name = name;
    }
}