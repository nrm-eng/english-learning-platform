using Domain.Entities;

namespace Api.Dtos;

public record PlacementTestDto(
    int Id,
    int UserId,
    int LevelId,
    int Score,
    int MaxScore,
    DateTime TakenAt)
{
    public static PlacementTestDto FromDomainModel(PlacementTest model)
        => new(
            model.Id,
            model.UserId,
            model.LevelId,
            model.Score,
            model.MaxScore,
            model.TakenAt);
}

public record SubmitPlacementTestDto(
    int UserId,
    int LevelId,
    int Score,
    int MaxScore);