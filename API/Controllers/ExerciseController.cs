using Api.Dtos;
using Api.Modules.Errors;
using Application.Exercises.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("exercises")]
[ApiController]
public class ExerciseController(
    ISender sender,
    IExerciseQueries exerciseQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExerciseDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var exercises = await exerciseQueries.GetAllAsync(cancellationToken);
        return Ok(exercises.Select(ExerciseDto.FromDomainModel).ToList());
    }

    [HttpGet("{exerciseId:int}")]
    public async Task<ActionResult<ExerciseDto>> GetById(
        [FromRoute] int exerciseId,
        CancellationToken cancellationToken)
    {
        var exercise = await exerciseQueries.GetByIdAsync(exerciseId, cancellationToken);
        return exercise.Match<ActionResult<ExerciseDto>>(
            e => Ok(ExerciseDto.FromDomainModel(e)),
            () => NotFound());
    }

    [HttpGet("level/{levelId:int}")]
    public async Task<ActionResult<IReadOnlyList<ExerciseDto>>> GetByLevel(
        [FromRoute] int levelId,
        CancellationToken cancellationToken)
    {
        var exercises = await exerciseQueries.GetByLevelIdAsync(levelId, cancellationToken);
        return Ok(exercises.Select(ExerciseDto.FromDomainModel).ToList());
    }

    [HttpGet("type/{typeId:int}")]
    public async Task<ActionResult<IReadOnlyList<ExerciseDto>>> GetByType(
        [FromRoute] int typeId,
        CancellationToken cancellationToken)
    {
        var exercises = await exerciseQueries.GetByTypeIdAsync(typeId, cancellationToken);
        return Ok(exercises.Select(ExerciseDto.FromDomainModel).ToList());
    }

    [HttpGet("level/{levelId:int}/type/{typeId:int}")]
    public async Task<ActionResult<IReadOnlyList<ExerciseDto>>> GetByLevelAndType(
        [FromRoute] int levelId,
        [FromRoute] int typeId,
        CancellationToken cancellationToken)
    {
        var exercises = await exerciseQueries.GetByLevelAndTypeAsync(levelId, typeId, cancellationToken);
        return Ok(exercises.Select(ExerciseDto.FromDomainModel).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseDto>> Create(
        [FromBody] CreateExerciseDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateExerciseCommand
        {
            LevelId = request.LevelId,
            TypeId = request.TypeId,
            Title = request.Title,
            Content = request.Content
        }, cancellationToken);

        return result.Match<ActionResult<ExerciseDto>>(
            e => Ok(ExerciseDto.FromDomainModel(e)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<ExerciseDto>> Update(
        [FromBody] UpdateExerciseDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateExerciseCommand
        {
            Id = request.Id,
            LevelId = request.LevelId,
            TypeId = request.TypeId,
            Title = request.Title,
            Content = request.Content
        }, cancellationToken);

        return result.Match<ActionResult<ExerciseDto>>(
            e => Ok(ExerciseDto.FromDomainModel(e)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{exerciseId:int}")]
    public async Task<ActionResult<ExerciseDto>> Delete(
        [FromRoute] int exerciseId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteExerciseCommand
        {
            Id = exerciseId
        }, cancellationToken);

        return result.Match<ActionResult<ExerciseDto>>(
            e => Ok(ExerciseDto.FromDomainModel(e)),
            e => e.ToObjectResult());
    }
}