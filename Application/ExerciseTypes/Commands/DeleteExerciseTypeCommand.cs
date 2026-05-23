using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.ExerciseTypes.Commands;

public class DeleteExerciseTypeCommand : IRequest<Either<BaseException, ExerciseType>>
{
    public required int Id { get; init; }
}

public class DeleteExerciseTypeCommandHandler(
    IRepository<ExerciseType> exerciseTypeRepository,
    IExerciseTypeQueries exerciseTypeQueries,
    IExerciseQueries exerciseQueries) : IRequestHandler<DeleteExerciseTypeCommand, Either<BaseException, ExerciseType>>
{
    public async Task<Either<BaseException, ExerciseType>> Handle(
        DeleteExerciseTypeCommand request, CancellationToken cancellationToken)
    {
        var exerciseType = await exerciseTypeQueries.GetByIdAsync(request.Id, cancellationToken);
        return await exerciseType.MatchAsync(
            t => CheckDependencies(t.Id, cancellationToken)
                .BindAsync(_ => DeleteEntity(t, cancellationToken)),
            () => new ExerciseTypeNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
    int exerciseTypeId, CancellationToken cancellationToken)
    {
        var exercises = await exerciseQueries.GetByTypeIdAsync(exerciseTypeId, cancellationToken);
        return exercises != null && exercises.Any()
            ? new ExerciseTypeHasExercisesException(exerciseTypeId)
            : LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, ExerciseType>> DeleteEntity(
        ExerciseType exerciseType, CancellationToken cancellationToken)
    {
        try
        {
            return await exerciseTypeRepository.DeleteAsync(exerciseType, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledExerciseTypeException(exerciseType.Id, ex);
        }
    }
}