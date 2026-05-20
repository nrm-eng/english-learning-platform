using Api.Dtos;
using Api.Modules.Errors;
using Application.Questions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("questions")]
[ApiController]
public class QuestionController(
    ISender sender,
    IQuestionQueries questionQueries) : ControllerBase
{
    [HttpGet("{questionId:int}")]
    public async Task<ActionResult<QuestionDto>> GetById(
        [FromRoute] int questionId,
        CancellationToken cancellationToken)
    {
        var question = await questionQueries.GetByIdAsync(questionId, cancellationToken);
        return question.Match<ActionResult<QuestionDto>>(
            q => Ok(QuestionDto.FromDomainModel(q)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<QuestionDto>> Create(
        [FromBody] CreateQuestionDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateQuestionCommand
        {
            ExerciseId = request.ExerciseId,
            Text = request.Text
        }, cancellationToken);

        return result.Match<ActionResult<QuestionDto>>(
            q => Ok(QuestionDto.FromDomainModel(q)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<QuestionDto>> Update(
        [FromBody] UpdateQuestionDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateQuestionCommand
        {
            Id = request.Id,
            Text = request.Text
        }, cancellationToken);

        return result.Match<ActionResult<QuestionDto>>(
            q => Ok(QuestionDto.FromDomainModel(q)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{questionId:int}")]
    public async Task<ActionResult<QuestionDto>> Delete(
        [FromRoute] int questionId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteQuestionCommand
        {
            Id = questionId
        }, cancellationToken);

        return result.Match<ActionResult<QuestionDto>>(
            q => Ok(QuestionDto.FromDomainModel(q)),
            e => e.ToObjectResult());
    }
}