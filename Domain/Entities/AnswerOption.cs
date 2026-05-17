using Domain.Interfaces;

namespace Domain.Entities;

public class AnswerOption : IEntity
{
    public int Id { get; private set; }
    public int QuestionId { get; private set; }
    public string Text { get; private set; }
    public bool IsCorrect { get; private set; }

    public Question? Question { get; private set; }

    private AnswerOption(int questionId, string text, bool isCorrect)
    {
        QuestionId = questionId;
        Text = text;
        IsCorrect = isCorrect;
    }

    public static AnswerOption New(int questionId, string text, bool isCorrect)
        => new AnswerOption(questionId, text, isCorrect);

    public void UpdateDetails(string text, bool isCorrect)
    {
        Text = text;
        IsCorrect = isCorrect;
    }
}