using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using MediatR;

namespace Application.PlacementTests.Commands;

public class SubmitPlacementTestCommand : IRequest<Either<BaseException, PlacementTest>>
{
    public required int UserId { get; init; }
    public required int LevelId { get; init; }
    public required int Score { get; init; }
    public required int MaxScore { get; init; }
}

public class SubmitPlacementTestCommandHandler(
    IRepository<PlacementTest> placementTestRepository,
    IRepository<User> userRepository,
    IPlacementTestQueries placementTestQueries,
    IUserQueries userQueries,
    ILevelQueries levelQueries) : IRequestHandler<SubmitPlacementTestCommand, Either<BaseException, PlacementTest>>
{
    public async Task<Either<BaseException, PlacementTest>> Handle(
        SubmitPlacementTestCommand request, CancellationToken cancellationToken)
    {
        return await CheckDependencies(request, cancellationToken)
            .BindAsync(_ => CreateOrUpdate(request, cancellationToken));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        SubmitPlacementTestCommand request, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByIdAsync(request.UserId, cancellationToken);
        if (user.IsNone)
            return new UserNotFoundException(request.UserId);

        var level = await levelQueries.GetByIdAsync(request.LevelId, cancellationToken);
        if (level.IsNone)
            return new LevelNotFoundException(request.LevelId);

        return LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, PlacementTest>> CreateOrUpdate(
    SubmitPlacementTestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // оновлюємо рівень юзера
            var user = await userQueries.GetByIdAsync(request.UserId, cancellationToken);
            if (user.IsSome)
            {
                var u = user.ValueUnsafe();
                u.SetLevel(request.LevelId);
                await userRepository.UpdateAsync(u, cancellationToken);
            }

            // створюємо або оновлюємо результат тесту
            var existingTest = await placementTestQueries
                .GetByUserIdAsync(request.UserId, cancellationToken);

            return await existingTest.MatchAsync(
                async t =>
                {
                    t.UpdateResult(request.LevelId, request.Score, request.MaxScore);
                    return await placementTestRepository.UpdateAsync(t, cancellationToken);
                },
                async () => await placementTestRepository.CreateAsync(
                    PlacementTest.New(request.UserId, request.LevelId, request.Score, request.MaxScore),
                    cancellationToken));
        }
        catch (Exception ex)
        {
            return new UnhandledPlacementTestException(0, ex);
        }
    }
}