using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.AnswerOptions.Commands;

public class DeleteAnswerOptionCommand : IRequest<Either<BaseException, AnswerOption>>
{
    public required int Id { get; init; }
}

public class DeleteAnswerOptionCommandHandler(
    IRepository<AnswerOption> answerOptionRepository,
    IAnswerOptionQueries answerOptionQueries) : IRequestHandler<DeleteAnswerOptionCommand, Either<BaseException, AnswerOption>>
{
    public async Task<Either<BaseException, AnswerOption>> Handle(
        DeleteAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        var answerOption = await answerOptionQueries.GetByIdAsync(request.Id, cancellationToken);
        return await answerOption.MatchAsync(
            a => DeleteEntity(a, cancellationToken),
            () => new AnswerOptionNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, AnswerOption>> DeleteEntity(
        AnswerOption answerOption, CancellationToken cancellationToken)
    {
        try
        {
            return await answerOptionRepository.DeleteAsync(answerOption, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledAnswerOptionException(answerOption.Id, ex);
        }
    }
}