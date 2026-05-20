using Domain.Entities;

namespace Api.Dtos;

public record RoleDto(
    int Id,
    string Name)
{
    public static RoleDto FromDomainModel(Role model)
        => new(model.Id, model.Name);
}

public record CreateRoleDto(string Name);

public record UpdateRoleDto(
    int Id,
    string Name);