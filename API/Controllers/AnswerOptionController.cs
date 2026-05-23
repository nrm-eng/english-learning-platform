using Api.Dtos;
using Api.Modules.Errors;
using Application.AnswerOptions.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("answer-options")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AnswerOptionController(
    ISender sender,
    IAnswerOptionQueries answerOptionQueries) : ControllerBase
{
    [HttpGet("{answerOptionId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<AnswerOptionDto>> GetById(
        [FromRoute] int answerOptionId,
        CancellationToken cancellationToken)
    {
        var answerOption = await answerOptionQueries.GetByIdAsync(answerOptionId, cancellationToken);
        return answerOption.Match<ActionResult<AnswerOptionDto>>(
            a => Ok(AnswerOptionDto.FromDomainModel(a)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<AnswerOptionDto>> Create(
        [FromBody] CreateAnswerOptionDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateAnswerOptionCommand
        {
            QuestionId = request.QuestionId,
            Text = request.Text,
            IsCorrect = request.IsCorrect
        }, cancellationToken);

        return result.Match<ActionResult<AnswerOptionDto>>(
            a => Ok(AnswerOptionDto.FromDomainModel(a)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<AnswerOptionDto>> Update(
        [FromBody] UpdateAnswerOptionDto request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateAnswerOptionCommand
        {
            Id = request.Id,
            Text = request.Text,
            IsCorrect = request.IsCorrect
        }, cancellationToken);

        return result.Match<ActionResult<AnswerOptionDto>>(
            a => Ok(AnswerOptionDto.FromDomainModel(a)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{answerOptionId:int}")]
    public async Task<ActionResult<AnswerOptionDto>> Delete(
        [FromRoute] int answerOptionId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteAnswerOptionCommand
        {
            Id = answerOptionId
        }, cancellationToken);

        return result.Match<ActionResult<AnswerOptionDto>>(
            a => Ok(AnswerOptionDto.FromDomainModel(a)),
            e => e.ToObjectResult());
    }
}