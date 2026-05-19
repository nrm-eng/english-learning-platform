using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Exercises.Commands;

public class CreateExerciseCommand : IRequest<Either<BaseException, Exercise>>
{
    public required int LevelId { get; init; }
    public required int TypeId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}

public class CreateExerciseCommandHandler(
    IRepository<Exercise> exerciseRepository,
    ILevelQueries levelQueries,
    IExerciseTypeQueries exerciseTypeQueries) : IRequestHandler<CreateExerciseCommand, Either<BaseException, Exercise>>
{
    public async Task<Either<BaseException, Exercise>> Handle(
        CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        return await CheckDependencies(request, cancellationToken)
            .BindAsync(_ => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var level = await levelQueries.GetByIdAsync(request.LevelId, cancellationToken);
        if (level.IsNone)
            return new LevelNotFoundException(request.LevelId);

        var exerciseType = await exerciseTypeQueries.GetByIdAsync(request.TypeId, cancellationToken);
        if (exerciseType.IsNone)
            return new ExerciseTypeNotFoundException(request.TypeId);

        return LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, Exercise>> CreateEntity(
        CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exercise = await exerciseRepository.CreateAsync(
                Exercise.New(request.LevelId, request.TypeId, request.Title, request.Content),
                cancellationToken);
            return exercise;
        }
        catch (Exception ex)
        {
            return new UnhandledExerciseException(0, ex);
        }
    }
}