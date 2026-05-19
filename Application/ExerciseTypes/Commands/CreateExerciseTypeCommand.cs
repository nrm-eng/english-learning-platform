using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.ExerciseTypes.Commands;

public class CreateExerciseTypeCommand : IRequest<Either<BaseException, ExerciseType>>
{
    public required string Name { get; init; }
}

public class CreateExerciseTypeCommandHandler(
    IRepository<ExerciseType> exerciseTypeRepository,
    IExerciseTypeQueries exerciseTypeQueries) : IRequestHandler<CreateExerciseTypeCommand, Either<BaseException, ExerciseType>>
{
    public async Task<Either<BaseException, ExerciseType>> Handle(
        CreateExerciseTypeCommand request, CancellationToken cancellationToken)
    {
        var existingType = await exerciseTypeQueries.GetByNameAsync(request.Name, cancellationToken);
        return await existingType.MatchAsync(
            t => new ExerciseTypeAlreadyExistsException(t.Name),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, ExerciseType>> CreateEntity(
        CreateExerciseTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exerciseType = await exerciseTypeRepository.CreateAsync(
                ExerciseType.New(request.Name),
                cancellationToken);
            return exerciseType;
        }
        catch (Exception ex)
        {
            return new UnhandledExerciseTypeException(0, ex);
        }
    }
}