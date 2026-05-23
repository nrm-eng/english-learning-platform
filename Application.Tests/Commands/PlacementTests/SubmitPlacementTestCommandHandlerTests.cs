using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.PlacementTests.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.PlacementTests;

public class SubmitPlacementTestCommandHandlerTests
{
    private readonly Mock<IRepository<PlacementTest>> _placementTestRepositoryMock = new();
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IPlacementTestQueries> _placementTestQueriesMock = new();
    private readonly Mock<IUserQueries> _userQueriesMock = new();
    private readonly Mock<ILevelQueries> _levelQueriesMock = new();
    private readonly SubmitPlacementTestCommandHandler _handler;

    public SubmitPlacementTestCommandHandlerTests()
    {
        _handler = new SubmitPlacementTestCommandHandler(
            _placementTestRepositoryMock.Object,
            _userRepositoryMock.Object,
            _placementTestQueriesMock.Object,
            _userQueriesMock.Object,
            _levelQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFoundException()
    {
        var command = CreateValidCommand();

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UserNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenLevelDoesNotExist_ReturnsLevelNotFoundException()
    {
        var command = CreateValidCommand();
        var user = User.New("Roman", "roman@gmail.com", "hash", 1);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<LevelNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenFirstTest_CreatesNewPlacementTest()
    {
        var command = CreateValidCommand();
        var user = User.New("Roman", "roman@gmail.com", "hash", 1);
        var level = Level.New(Domain.Enums.CefrLevel.B1, "Intermediate");
        var expectedTest = PlacementTest.New(command.UserId, command.LevelId, command.Score, command.MaxScore);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.Some(level));
        _placementTestQueriesMock
            .Setup(x => x.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<PlacementTest>.None);
        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _placementTestRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<PlacementTest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTest);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        _placementTestRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<PlacementTest>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTestAlreadyExists_UpdatesExistingTest()
    {
        var command = CreateValidCommand();
        var user = User.New("Roman", "roman@gmail.com", "hash", 1);
        var level = Level.New(Domain.Enums.CefrLevel.B1, "Intermediate");
        var existingTest = PlacementTest.New(command.UserId, 1, 10, 20);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.Some(level));
        _placementTestQueriesMock
            .Setup(x => x.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<PlacementTest>.Some(existingTest));
        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _placementTestRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<PlacementTest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTest);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        _placementTestRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<PlacementTest>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static SubmitPlacementTestCommand CreateValidCommand() => new()
    {
        UserId = 1,
        LevelId = 3,
        Score = 18,
        MaxScore = 25
    };
}