using Domain.Entities;

namespace Api.Dtos;

public record ExerciseTypeDto(
    int Id,
    string Name)
{
    public static ExerciseTypeDto FromDomainModel(ExerciseType model)
        => new(model.Id, model.Name);
}

public record CreateExerciseTypeDto(string Name);

public record UpdateExerciseTypeDto(
    int Id,
    string Name);