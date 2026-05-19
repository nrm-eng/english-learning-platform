using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.AnswerOptions.Commands;

public class CreateAnswerOptionCommand : IRequest<Either<BaseException, AnswerOption>>
{
    public required int QuestionId { get; init; }
    public required string Text { get; init; }
    public required bool IsCorrect { get; init; }
}

public class CreateAnswerOptionCommandHandler(
    IRepository<AnswerOption> answerOptionRepository,
    IQuestionQueries questionQueries) : IRequestHandler<CreateAnswerOptionCommand, Either<BaseException, AnswerOption>>
{
    public async Task<Either<BaseException, AnswerOption>> Handle(
        CreateAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionQueries.GetByIdAsync(request.QuestionId, cancellationToken);
        return await question.MatchAsync(
            q => CreateEntity(request, cancellationToken),
            () => new QuestionNotFoundException(request.QuestionId));
    }

    private async Task<Either<BaseException, AnswerOption>> CreateEntity(
        CreateAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var answerOption = await answerOptionRepository.CreateAsync(
                AnswerOption.New(request.QuestionId, request.Text, request.IsCorrect),
                cancellationToken);
            return answerOption;
        }
        catch (Exception ex)
        {
            return new UnhandledAnswerOptionException(0, ex);
        }
    }
}