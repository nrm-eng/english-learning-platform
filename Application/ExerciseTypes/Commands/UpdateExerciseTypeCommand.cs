using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.ExerciseTypes.Commands;

public class UpdateExerciseTypeCommand : IRequest<Either<BaseException, ExerciseType>>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateExerciseTypeCommandHandler(
    IRepository<ExerciseType> exerciseTypeRepository,
    IExerciseTypeQueries exerciseTypeQueries) : IRequestHandler<UpdateExerciseTypeCommand, Either<BaseException, ExerciseType>>
{
    public async Task<Either<BaseException, ExerciseType>> Handle(
        UpdateExerciseTypeCommand request, CancellationToken cancellationToken)
    {
        var exerciseType = await exerciseTypeQueries.GetByIdAsync(request.Id, cancellationToken);
        return await exerciseType.MatchAsync(
            t => CheckDuplicates(t.Id, request.Name, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, t, cancellationToken)),
            () => new ExerciseTypeNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, ExerciseType>> UpdateEntity(
        UpdateExerciseTypeCommand request, ExerciseType exerciseType, CancellationToken cancellationToken)
    {
        try
        {
            exerciseType.UpdateDetails(request.Name);
            return await exerciseTypeRepository.UpdateAsync(exerciseType, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledExerciseTypeException(exerciseType.Id, ex);
        }
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDuplicates(
        int currentTypeId, string name, CancellationToken cancellationToken)
    {
        var exerciseType = await exerciseTypeQueries.GetByNameAsync(name, cancellationToken);
        return exerciseType.Match<Either<BaseException, LanguageExt.Unit>>(
            t => t.Id == currentTypeId
                ? LanguageExt.Unit.Default
                : new ExerciseTypeAlreadyExistsException(name),
            () => LanguageExt.Unit.Default);
    }
}