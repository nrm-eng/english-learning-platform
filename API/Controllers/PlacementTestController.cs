using Api.Dtos;
using Api.Modules.Errors;
using Application.PlacementTests.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("placement-tests")]
[ApiController]
[Authorize]
public class PlacementTestController(
    ISender sender,
    IPlacementTestQueries placementTestQueries) : ControllerBase
{
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<PlacementTestDto>> GetByUser(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var result = await placementTestQueries.GetByUserIdAsync(userId, cancellationToken);
        return result.Match<ActionResult<PlacementTestDto>>(
            t => Ok(PlacementTestDto.FromDomainModel(t)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<PlacementTestDto>> Submit(
        [FromBody] SubmitPlacementTestDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SubmitPlacementTestCommand
        {
            UserId = request.UserId,
            LevelId = request.LevelId,
            Score = request.Score,
            MaxScore = request.MaxScore
        }, cancellationToken);

        return result.Match<ActionResult<PlacementTestDto>>(
            t => Ok(PlacementTestDto.FromDomainModel(t)),
            e => e.ToObjectResult());
    }
}