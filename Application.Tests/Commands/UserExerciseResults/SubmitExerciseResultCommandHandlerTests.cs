using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.UserExerciseResults.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.UserExerciseResults;

public class SubmitExerciseResultCommandHandlerTests
{
    private readonly Mock<IRepository<UserExerciseResult>> _resultRepositoryMock = new();
    private readonly Mock<IUserExerciseResultQueries> _resultQueriesMock = new();
    private readonly Mock<IUserQueries> _userQueriesMock = new();
    private readonly Mock<IExerciseQueries> _exerciseQueriesMock = new();
    private readonly SubmitExerciseResultCommandHandler _handler;

    public SubmitExerciseResultCommandHandlerTests()
    {
        _handler = new SubmitExerciseResultCommandHandler(
            _resultRepositoryMock.Object,
            _resultQueriesMock.Object,
            _userQueriesMock.Object,
            _exerciseQueriesMock.Object);
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
    public async Task Handle_WhenExerciseDoesNotExist_ReturnsExerciseNotFoundException()
    {
        var command = CreateValidCommand();
        var user = User.New("Roman", "roman@gmail.com", "hash", 1);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.ExerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<ExerciseNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenFirstAttempt_CreatesNewResult()
    {
        var command = CreateValidCommand();
        var user = User.New("Roman", "roman@gmail.com", "hash", 1);
        var exercise = Exercise.New(1, 1, "Test", "Content");
        var expectedResult = UserExerciseResult.New(command.UserId, command.ExerciseId, command.Score, command.MaxScore);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.ExerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
        _resultQueriesMock
            .Setup(x => x.GetByUserAndExerciseAsync(command.UserId, command.ExerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<UserExerciseResult>.None);
        _resultRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<UserExerciseResult>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        _resultRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<UserExerciseResult>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenResultAlreadyExists_UpdatesExistingResult()
    {
        var command = CreateValidCommand();
        var user = User.New("Roman", "roman@gmail.com", "hash", 1);
        var exercise = Exercise.New(1, 1, "Test", "Content");
        var existingResult = UserExerciseResult.New(command.UserId, command.ExerciseId, 0, 1);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.ExerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
        _resultQueriesMock
            .Setup(x => x.GetByUserAndExerciseAsync(command.UserId, command.ExerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<UserExerciseResult>.Some(existingResult));
        _resultRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<UserExerciseResult>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        _resultRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<UserExerciseResult>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static SubmitExerciseResultCommand CreateValidCommand() => new()
    {
        UserId = 1,
        ExerciseId = 1,
        Score = 1,
        MaxScore = 1
    };
}