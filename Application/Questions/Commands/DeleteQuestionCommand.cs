using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Questions.Commands;

public class DeleteQuestionCommand : IRequest<Either<BaseException, Question>>
{
    public required int Id { get; init; }
}

public class DeleteQuestionCommandHandler(
    IRepository<Question> questionRepository,
    IQuestionQueries questionQueries) : IRequestHandler<DeleteQuestionCommand, Either<BaseException, Question>>
{
    public async Task<Either<BaseException, Question>> Handle(
        DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionQueries.GetByIdAsync(request.Id, cancellationToken);
        return await question.MatchAsync(
            q => DeleteEntity(q, cancellationToken),
            () => new QuestionNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Question>> DeleteEntity(
        Question question, CancellationToken cancellationToken)
    {
        try
        {
            return await questionRepository.DeleteAsync(question, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledQuestionException(question.Id, ex);
        }
    }
}