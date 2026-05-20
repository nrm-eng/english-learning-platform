using Domain.Entities;

namespace Api.Dtos;

public record AnswerOptionDto(
    int Id,
    int QuestionId,
    string Text,
    bool IsCorrect)
{
    public static AnswerOptionDto FromDomainModel(AnswerOption model)
        => new(
            model.Id,
            model.QuestionId,
            model.Text,
            model.IsCorrect);
}

public record CreateAnswerOptionDto(
    int QuestionId,
    string Text,
    bool IsCorrect);

public record UpdateAnswerOptionDto(
    int Id,
    string Text,
    bool IsCorrect);