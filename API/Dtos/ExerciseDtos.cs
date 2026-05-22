using Domain.Entities;

namespace Api.Dtos;

public record ExerciseDto(
    int Id,
    int LevelId,
    int TypeId,
    string Title,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static ExerciseDto FromDomainModel(Exercise model)
        => new(
            model.Id,
            model.LevelId,
            model.TypeId,
            model.Title,
            model.Content,
            model.CreatedAt,
            model.UpdatedAt);
}

public record ExerciseDetailDto(
    int Id,
    int LevelId,
    int TypeId,
    string Title,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<QuestionDto>? Questions)
{
    public static ExerciseDetailDto FromDomainModel(Exercise model)
        => new(
            model.Id,
            model.LevelId,
            model.TypeId,
            model.Title,
            model.Content,
            model.CreatedAt,
            model.UpdatedAt,
            model.Questions?.Select(QuestionDto.FromDomainModel).ToList());
}

public record CreateExerciseDto(
    int LevelId,
    int TypeId,
    string Title,
    string Content);

public record UpdateExerciseDto(
    int Id,
    int LevelId,
    int TypeId,
    string Title,
    string Content);