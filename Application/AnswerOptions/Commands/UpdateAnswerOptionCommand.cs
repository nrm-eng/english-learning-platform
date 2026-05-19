using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.AnswerOptions.Commands;

public class UpdateAnswerOptionCommand : IRequest<Either<BaseException, AnswerOption>>
{
    public required int Id { get; init; }
    public required string Text { get; init; }
    public required bool IsCorrect { get; init; }
}

public class UpdateAnswerOptionCommandHandler(
    IRepository<AnswerOption> answerOptionRepository,
    IAnswerOptionQueries answerOptionQueries) : IRequestHandler<UpdateAnswerOptionCommand, Either<BaseException, AnswerOption>>
{
    public async Task<Either<BaseException, AnswerOption>> Handle(
        UpdateAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        var answerOption = await answerOptionQueries.GetByIdAsync(request.Id, cancellationToken);
        return await answerOption.MatchAsync(
            a => UpdateEntity(request, a, cancellationToken),
            () => new AnswerOptionNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, AnswerOption>> UpdateEntity(
        UpdateAnswerOptionCommand request, AnswerOption answerOption, CancellationToken cancellationToken)
    {
        try
        {
            answerOption.UpdateDetails(request.Text, request.IsCorrect);
            return await answerOptionRepository.UpdateAsync(answerOption, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledAnswerOptionException(answerOption.Id, ex);
        }
    }
}