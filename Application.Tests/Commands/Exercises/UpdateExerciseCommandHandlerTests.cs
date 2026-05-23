using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Exercises.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.Exercises;

public class UpdateExerciseCommandHandlerTests
{
    private readonly Mock<IRepository<Exercise>> _exerciseRepositoryMock = new();
    private readonly Mock<IExerciseQueries> _exerciseQueriesMock = new();
    private readonly Mock<ILevelQueries> _levelQueriesMock = new();
    private readonly Mock<IExerciseTypeQueries> _exerciseTypeQueriesMock = new();
    private readonly UpdateExerciseCommandHandler _handler;

    public UpdateExerciseCommandHandlerTests()
    {
        _handler = new UpdateExerciseCommandHandler(
            _exerciseRepositoryMock.Object,
            _exerciseQueriesMock.Object,
            _levelQueriesMock.Object,
            _exerciseTypeQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenExerciseDoesNotExist_ReturnsExerciseNotFoundException()
    {
        var command = CreateValidCommand();

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<ExerciseNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenLevelDoesNotExist_ReturnsLevelNotFoundException()
    {
        var command = CreateValidCommand();
        var exercise = Exercise.New(1, 1, "Title", "Content");

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
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
        var exercise = Exercise.New(1, 1, "Title", "Content");
        var level = Level.New(Domain.Enums.CefrLevel.A1, "Beginner");

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
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
    public async Task Handle_WhenValid_ReturnsUpdatedExercise()
    {
        var command = CreateValidCommand();
        var exercise = Exercise.New(1, 1, "Title", "Content");
        var level = Level.New(Domain.Enums.CefrLevel.A1, "Beginner");
        var exerciseType = ExerciseType.New("Reading");

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
        _levelQueriesMock
            .Setup(x => x.GetByIdAsync(command.LevelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Level>.Some(level));
        _exerciseTypeQueriesMock
            .Setup(x => x.GetByIdAsync(command.TypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<ExerciseType>.Some(exerciseType));
        _exerciseRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercise);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        _exerciseRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static UpdateExerciseCommand CreateValidCommand() => new()
    {
        Id = 1,
        LevelId = 1,
        TypeId = 1,
        Title = "Updated Title",
        Content = "Updated Content"
    };
}