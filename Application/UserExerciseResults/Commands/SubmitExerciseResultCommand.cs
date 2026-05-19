using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.UserExerciseResults.Commands;

public class SubmitExerciseResultCommand : IRequest<Either<BaseException, UserExerciseResult>>
{
    public required int UserId { get; init; }
    public required int ExerciseId { get; init; }
    public required int Score { get; init; }
    public required int MaxScore { get; init; }
}

public class SubmitExerciseResultCommandHandler(
    IRepository<UserExerciseResult> userExerciseResultRepository,
    IUserExerciseResultQueries userExerciseResultQueries,
    IUserQueries userQueries,
    IExerciseQueries exerciseQueries) : IRequestHandler<SubmitExerciseResultCommand, Either<BaseException, UserExerciseResult>>
{
    public async Task<Either<BaseException, UserExerciseResult>> Handle(
        SubmitExerciseResultCommand request, CancellationToken cancellationToken)
    {
        return await CheckDependencies(request, cancellationToken)
            .BindAsync(_ => CreateOrUpdate(request, cancellationToken));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        SubmitExerciseResultCommand request, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByIdAsync(request.UserId, cancellationToken);
        if (user.IsNone)
            return new UserNotFoundException(request.UserId);

        var exercise = await exerciseQueries.GetByIdAsync(request.ExerciseId, cancellationToken);
        if (exercise.IsNone)
            return new ExerciseNotFoundException(request.ExerciseId);

        return LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, UserExerciseResult>> CreateOrUpdate(
        SubmitExerciseResultCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingResult = await userExerciseResultQueries
                .GetByUserAndExerciseAsync(request.UserId, request.ExerciseId, cancellationToken);

            return await existingResult.MatchAsync(
                async r =>
                {
                    r.UpdateResult(request.Score, request.MaxScore);
                    return await userExerciseResultRepository.UpdateAsync(r, cancellationToken);
                },
                async () =>
                {
                    return await userExerciseResultRepository.CreateAsync(
                        UserExerciseResult.New(request.UserId, request.ExerciseId, request.Score, request.MaxScore),
                        cancellationToken);
                });
        }
        catch (Exception ex)
        {
            return new UnhandledUserExerciseResultException(0, ex);
        }
    }
}