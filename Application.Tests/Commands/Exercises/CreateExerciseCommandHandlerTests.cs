using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Exercises.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.Exercises;

public class CreateExerciseCommandHandlerTests
{
    private readonly Mock<IRepository<Exercise>> _exerciseRepositoryMock = new();
    private readonly Mock<ILevelQueries> _levelQueriesMock = new();
    private readonly Mock<IExerciseTypeQueries> _exerciseTypeQueriesMock = new();
    private readonly CreateExerciseCommandHandler _handler;

    public CreateExerciseCommandHandlerTests()
    {
        _handler = new CreateExerciseCommandHandler(
            _exerciseRepositoryMock.Object,
            _levelQueriesMock.Object,
            _exerciseTypeQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenLevelDoesNotExist_ReturnsLevelNotFoundException()
    {
        var command = CreateValidCommand();

        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<LevelNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenExerciseTypeDoesNotExist_ReturnsExerciseTypeNotFoundException()
    {
        var command = CreateValidCommand();
        var level = Level.New(Domain.Enums.CefrLevel.A1, "Beginner");

        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.Some(level));
        _exerciseTypeQueriesMock
            .Setup(x => x.GetByIdAsync(command.TypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<ExerciseType>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<ExerciseTypeNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenValid_ReturnsCreatedExercise()
    {
        var command = CreateValidCommand();
        var level = Level.New(Domain.Enums.CefrLevel.A1, "Beginner");
        var exerciseType = ExerciseType.New("Reading");
        var expectedExercise = Exercise.New(command.LevelId, command.TypeId, command.Title, command.Content);

        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.Some(level));
        _exerciseTypeQueriesMock
            .Setup(x => x.GetByIdAsync(command.TypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<ExerciseType>.Some(exerciseType));
        _exerciseRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedExercise);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        result.IfRight(exercise => Assert.Equal(command.Title, exercise.Title));
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsUnhandledExerciseException()
    {
        var command = CreateValidCommand();
        var level = Level.New(Domain.Enums.CefrLevel.A1, "Beginner");
        var exerciseType = ExerciseType.New("Reading");

        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.Some(level));
        _exerciseTypeQueriesMock
            .Setup(x => x.GetByIdAsync(command.TypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<ExerciseType>.Some(exerciseType));
        _exerciseRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UnhandledExerciseException>(ex));
    }

    private static CreateExerciseCommand CreateValidCommand() => new()
    {
        LevelId = 1,
        TypeId = 1,
        Title = "Test Exercise",
        Content = "Test Content"
    };
}