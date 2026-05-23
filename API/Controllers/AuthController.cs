using Api.Dtos;
using Api.Modules.Errors;
using Application.Users.Commands;
using Application.Users.Commands.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("auth")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
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

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(
        [FromBody] LoginUserDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        }, cancellationToken);

        return result.Match<ActionResult<string>>(
            token => Ok(new { token }),
            e => e.ToObjectResult());
    }
}