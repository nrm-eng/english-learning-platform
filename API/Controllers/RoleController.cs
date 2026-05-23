using Api.Dtos;
using Api.Modules.Errors;
using Application.Roles.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("roles")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RoleController(
    ISender sender,
    IRoleQueries roleQueries) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var roles = await roleQueries.GetAllAsync(cancellationToken);
        return Ok(roles.Select(RoleDto.FromDomainModel).ToList());
    }

    [HttpGet("{roleId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<RoleDto>> GetById(
        [FromRoute] int roleId,
        CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(roleId, cancellationToken);
        return role.Match<ActionResult<RoleDto>>(
            r => Ok(RoleDto.FromDomainModel(r)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(
        [FromBody] CreateRoleDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateRoleCommand
        {
            Name = request.Name
        }, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => Ok(RoleDto.FromDomainModel(r)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<RoleDto>> Update(
        [FromBody] UpdateRoleDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateRoleCommand
        {
            Id = request.Id,
            Name = request.Name
        }, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => Ok(RoleDto.FromDomainModel(r)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{roleId:int}")]
    public async Task<ActionResult<RoleDto>> Delete(
        [FromRoute] int roleId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRoleCommand
        {
            Id = roleId
        }, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => Ok(RoleDto.FromDomainModel(r)),
            e => e.ToObjectResult());
    }
}