using Api.Dtos;
using Api.Modules.Errors;
using Application.ExerciseTypes.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("exercise-types")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ExerciseTypeController(
    ISender sender,
    IExerciseTypeQueries exerciseTypeQueries) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<ExerciseTypeDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var types = await exerciseTypeQueries.GetAllAsync(cancellationToken);
        return Ok(types.Select(ExerciseTypeDto.FromDomainModel).ToList());
    }

    [HttpGet("{typeId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ExerciseTypeDto>> GetById(
        [FromRoute] int typeId,
        CancellationToken cancellationToken)
    {
        var type = await exerciseTypeQueries.GetByIdAsync(typeId, cancellationToken);
        return type.Match<ActionResult<ExerciseTypeDto>>(
            t => Ok(ExerciseTypeDto.FromDomainModel(t)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseTypeDto>> Create(
        [FromBody] CreateExerciseTypeDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateExerciseTypeCommand
        {
            Name = request.Name
        }, cancellationToken);

        return result.Match<ActionResult<ExerciseTypeDto>>(
            t => Ok(ExerciseTypeDto.FromDomainModel(t)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<ExerciseTypeDto>> Update(
        [FromBody] UpdateExerciseTypeDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateExerciseTypeCommand
        {
            Id = request.Id,
            Name = request.Name
        }, cancellationToken);

        return result.Match<ActionResult<ExerciseTypeDto>>(
            t => Ok(ExerciseTypeDto.FromDomainModel(t)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{typeId:int}")]
    public async Task<ActionResult<ExerciseTypeDto>> Delete(
        [FromRoute] int typeId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteExerciseTypeCommand
        {
            Id = typeId
        }, cancellationToken);

        return result.Match<ActionResult<ExerciseTypeDto>>(
            t => Ok(ExerciseTypeDto.FromDomainModel(t)),
            e => e.ToObjectResult());
    }
}