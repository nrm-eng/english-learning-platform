using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Questions.Commands;

public class CreateQuestionCommand : IRequest<Either<BaseException, Question>>
{
    public required int ExerciseId { get; init; }
    public required string Text { get; init; }
}

public class CreateQuestionCommandHandler(
    IRepository<Question> questionRepository,
    IExerciseQueries exerciseQueries) : IRequestHandler<CreateQuestionCommand, Either<BaseException, Question>>
{
    public async Task<Either<BaseException, Question>> Handle(
        CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var exercise = await exerciseQueries.GetByIdAsync(request.ExerciseId, cancellationToken);
        return await exercise.MatchAsync(
            e => CreateEntity(request, cancellationToken),
            () => new ExerciseNotFoundException(request.ExerciseId));
    }

    private async Task<Either<BaseException, Question>> CreateEntity(
        CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var question = await questionRepository.CreateAsync(
                Question.New(request.ExerciseId, request.Text),
                cancellationToken);
            return question;
        }
        catch (Exception ex)
        {
            return new UnhandledQuestionException(0, ex);
        }
    }
}