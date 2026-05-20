using Domain.Entities;

namespace Api.Dtos;

public record QuestionDto(
    int Id,
    int ExerciseId,
    string Text,
    IReadOnlyList<AnswerOptionDto>? AnswerOptions)
{
    public static QuestionDto FromDomainModel(Question model)
        => new(
            model.Id,
            model.ExerciseId,
            model.Text,
            model.AnswerOptions != null
                ? model.AnswerOptions
                    .Select(AnswerOptionDto.FromDomainModel)
                    .ToList()
                : []);
}

public record CreateQuestionDto(
    int ExerciseId,
    string Text);

public record UpdateQuestionDto(
    int Id,
    string Text);