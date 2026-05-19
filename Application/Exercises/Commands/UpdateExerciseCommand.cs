using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Exercises.Commands;

public class UpdateExerciseCommand : IRequest<Either<BaseException, Exercise>>
{
    public required int Id { get; init; }
    public required int LevelId { get; init; }
    public required int TypeId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}

public class UpdateExerciseCommandHandler(
    IRepository<Exercise> exerciseRepository,
    IExerciseQueries exerciseQueries,
    ILevelQueries levelQueries,
    IExerciseTypeQueries exerciseTypeQueries) : IRequestHandler<UpdateExerciseCommand, Either<BaseException, Exercise>>
{
    public async Task<Either<BaseException, Exercise>> Handle(
        UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await exerciseQueries.GetByIdAsync(request.Id, cancellationToken);
        return await exercise.MatchAsync(
            e => CheckDependencies(request, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, e, cancellationToken)),
            () => new ExerciseNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var level = await levelQueries.GetByIdAsync(request.LevelId, cancellationToken);
        if (level.IsNone)
            return new LevelNotFoundException(request.LevelId);

        var exerciseType = await exerciseTypeQueries.GetByIdAsync(request.TypeId, cancellationToken);
        if (exerciseType.IsNone)
            return new ExerciseTypeNotFoundException(request.TypeId);

        return LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, Exercise>> UpdateEntity(
        UpdateExerciseCommand request, Exercise exercise, CancellationToken cancellationToken)
    {
        try
        {
            exercise.UpdateDetails(request.LevelId, request.TypeId, request.Title, request.Content);
            return await exerciseRepository.UpdateAsync(exercise, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledExerciseException(exercise.Id, ex);
        }
    }
}