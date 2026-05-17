using Domain.Interfaces;

namespace Domain.Entities;

public class Question : IEntity
{
    public int Id { get; private set; }
    public int ExerciseId { get; private set; }
    public string Text { get; private set; }

    public Exercise? Exercise { get; private set; }
    public ICollection<AnswerOption>? AnswerOptions { get; private set; } = [];

    private Question(int exerciseId, string text)
    {
        ExerciseId = exerciseId;
        Text = text;
    }

    public static Question New(int exerciseId, string text)
        => new Question(exerciseId, text);

    public void UpdateDetails(string text)
    {
        Text = text;
    }
}