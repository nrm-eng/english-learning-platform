using Domain.Entities;

namespace Api.Dtos;

public record UserExerciseResultDto(
    int Id,
    int UserId,
    int ExerciseId,
    int Score,
    int MaxScore,
    DateTime CompletedAt)
{
    public static UserExerciseResultDto FromDomainModel(UserExerciseResult model)
        => new(
            model.Id,
            model.UserId,
            model.ExerciseId,
            model.Score,
            model.MaxScore,
            model.CompletedAt);
}

public record SubmitExerciseResultDto(
    int UserId,
    int ExerciseId,
    int Score,
    int MaxScore);