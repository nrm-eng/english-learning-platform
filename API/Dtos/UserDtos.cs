using Domain.Entities;

namespace Api.Dtos;

public record UserDto(
    int Id,
    string Name,
    string Email,
    int RoleId,
    int? LevelId,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static UserDto FromDomainModel(User model)
        => new(
            model.Id,
            model.Name,
            model.Email,
            model.RoleId,
            model.LevelId,
            model.CreatedAt,
            model.UpdatedAt);
}

public record CreateUserDto(
    string Name,
    string Email,
    string Password);

public record UpdateUserDto(
    int Id,
    string Name,
    string Email,
    int RoleId);