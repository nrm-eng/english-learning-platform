using Api.Dtos;
using Api.Modules.Errors;
using Application.UserExerciseResults.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("user-exercise-results")]
[ApiController]
[Authorize]
public class UserExerciseResultController(
    ISender sender,
    IUserExerciseResultQueries userExerciseResultQueries) : ControllerBase
{
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<UserExerciseResultDto>>> GetByUser(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var results = await userExerciseResultQueries.GetByUserIdAsync(userId, cancellationToken);
        return Ok(results.Select(UserExerciseResultDto.FromDomainModel).ToList());
    }

    [HttpGet("user/{userId:int}/exercise/{exerciseId:int}")]
    public async Task<ActionResult<UserExerciseResultDto>> GetByUserAndExercise(
        [FromRoute] int userId,
        [FromRoute] int exerciseId,
        CancellationToken cancellationToken)
    {
        var result = await userExerciseResultQueries
            .GetByUserAndExerciseAsync(userId, exerciseId, cancellationToken);
        return result.Match<ActionResult<UserExerciseResultDto>>(
            r => Ok(UserExerciseResultDto.FromDomainModel(r)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<UserExerciseResultDto>> Submit(
        [FromBody] SubmitExerciseResultDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SubmitExerciseResultCommand
        {
            UserId = request.UserId,
            ExerciseId = request.ExerciseId,
            Score = request.Score,
            MaxScore = request.MaxScore
        }, cancellationToken);

        return result.Match<ActionResult<UserExerciseResultDto>>(
            r => Ok(UserExerciseResultDto.FromDomainModel(r)),
            e => e.ToObjectResult());
    }
}