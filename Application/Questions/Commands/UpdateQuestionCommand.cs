using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Questions.Commands;

public class UpdateQuestionCommand : IRequest<Either<BaseException, Question>>
{
    public required int Id { get; init; }
    public required string Text { get; init; }
}

public class UpdateQuestionCommandHandler(
    IRepository<Question> questionRepository,
    IQuestionQueries questionQueries) : IRequestHandler<UpdateQuestionCommand, Either<BaseException, Question>>
{
    public async Task<Either<BaseException, Question>> Handle(
        UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionQueries.GetByIdAsync(request.Id, cancellationToken);
        return await question.MatchAsync(
            q => UpdateEntity(request, q, cancellationToken),
            () => new QuestionNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Question>> UpdateEntity(
        UpdateQuestionCommand request, Question question, CancellationToken cancellationToken)
    {
        try
        {
            question.UpdateDetails(request.Text);
            return await questionRepository.UpdateAsync(question, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledQuestionException(question.Id, ex);
        }
    }
}