using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("levels")]
[ApiController]
public class LevelController(
    ILevelQueries levelQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LevelDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var levels = await levelQueries.GetAllAsync(cancellationToken);
        return Ok(levels.Select(LevelDto.FromDomainModel).ToList());
    }

    [HttpGet("{levelId:int}")]
    public async Task<ActionResult<LevelDto>> GetById(
        [FromRoute] int levelId,
        CancellationToken cancellationToken)
    {
        var level = await levelQueries.GetByIdAsync(levelId, cancellationToken);
        return level.Match<ActionResult<LevelDto>>(
            l => Ok(LevelDto.FromDomainModel(l)),
            () => NotFound());
    }
}