using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Exercises.Commands;

public class DeleteExerciseCommand : IRequest<Either<BaseException, Exercise>>
{
    public required int Id { get; init; }
}

public class DeleteExerciseCommandHandler(
    IRepository<Exercise> exerciseRepository,
    IExerciseQueries exerciseQueries) : IRequestHandler<DeleteExerciseCommand, Either<BaseException, Exercise>>
{
    public async Task<Either<BaseException, Exercise>> Handle(
        DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await exerciseQueries.GetByIdAsync(request.Id, cancellationToken);
        return await exercise.MatchAsync(
            e => DeleteEntity(e, cancellationToken),
            () => new ExerciseNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Exercise>> DeleteEntity(
        Exercise exercise, CancellationToken cancellationToken)
    {
        try
        {
            return await exerciseRepository.DeleteAsync(exercise, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledExerciseException(exercise.Id, ex);
        }
    }
}