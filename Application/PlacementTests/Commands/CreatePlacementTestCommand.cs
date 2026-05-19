using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.PlacementTests.Commands;

public class CreatePlacementTestCommand : IRequest<Either<BaseException, PlacementTest>>
{
    public required int UserId { get; init; }
    public required int LevelId { get; init; }
    public required int Score { get; init; }
    public required int MaxScore { get; init; }
}

public class CreatePlacementTestCommandHandler(
    IRepository<PlacementTest> placementTestRepository,
    IUserQueries userQueries,
    ILevelQueries levelQueries) : IRequestHandler<CreatePlacementTestCommand, Either<BaseException, PlacementTest>>
{
    public async Task<Either<BaseException, PlacementTest>> Handle(
        CreatePlacementTestCommand request, CancellationToken cancellationToken)
    {
        return await CheckDependencies(request, cancellationToken)
            .BindAsync(_ => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        CreatePlacementTestCommand request, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByIdAsync(request.UserId, cancellationToken);
        if (user.IsNone)
            return new UserNotFoundException(request.UserId);

        var level = await levelQueries.GetByIdAsync(request.LevelId, cancellationToken);
        if (level.IsNone)
            return new LevelNotFoundException(request.LevelId);

        return LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, PlacementTest>> CreateEntity(
        CreatePlacementTestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var placementTest = await placementTestRepository.CreateAsync(
                PlacementTest.New(request.UserId, request.LevelId, request.Score, request.MaxScore),
                cancellationToken);
            return placementTest;
        }
        catch (Exception ex)
        {
            return new UnhandledPlacementTestException(0, ex);
        }
    }
}