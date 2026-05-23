using Api.Dtos;
using Api.Modules.Errors;
using Application.Users.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserController(
    ISender sender,
    IUserQueries userQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var users = await userQueries.GetAllAsync(cancellationToken);
        return Ok(users.Select(UserDto.FromDomainModel).ToList());
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetById(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByIdAsync(userId, cancellationToken);
        return user.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            () => NotFound());
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetByEmail(
        [FromRoute] string email,
        CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByEmailAsync(email, cancellationToken);
        return user.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            () => NotFound());
    }

    [HttpGet("role/{roleId:int}")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetByRole(
        [FromRoute] int roleId,
        CancellationToken cancellationToken)
    {
        var users = await userQueries.GetByRoleIdAsync(roleId, cancellationToken);
        return Ok(users.Select(UserDto.FromDomainModel).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(
        [FromBody] CreateUserDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateUserCommand
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        }, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> Update(
        [FromBody] UpdateUserDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateUserCommand
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            RoleId = request.RoleId
        }, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult<UserDto>> Delete(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteUserCommand
        {
            Id = userId
        }, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }
}