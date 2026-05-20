using Domain.Entities;
using Domain.Enums;

namespace Api.Dtos;

public record LevelDto(
    int Id,
    CefrLevel CefrCode,
    string Name)
{
    public static LevelDto FromDomainModel(Level model)
        => new(model.Id, model.CefrCode, model.Name);
}