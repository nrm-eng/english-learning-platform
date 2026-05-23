using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Exercises.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.Exercises;

public class DeleteExerciseCommandHandlerTests
{
    private readonly Mock<IRepository<Exercise>> _exerciseRepositoryMock = new();
    private readonly Mock<IExerciseQueries> _exerciseQueriesMock = new();
    private readonly DeleteExerciseCommandHandler _handler;

    public DeleteExerciseCommandHandlerTests()
    {
        _handler = new DeleteExerciseCommandHandler(
            _exerciseRepositoryMock.Object,
            _exerciseQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenExerciseDoesNotExist_ReturnsExerciseNotFoundException()
    {
        var command = new DeleteExerciseCommand { Id = 999 };

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<ExerciseNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenExerciseExists_ReturnsDeletedExercise()
    {
        var exercise = Exercise.New(1, 1, "Test", "Content");
        var command = new DeleteExerciseCommand { Id = 1 };

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
        _exerciseRepositoryMock
            .Setup(x => x.DeleteAsync(exercise, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercise);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
    }

    [Fact]
    public async Task Handle_WhenExerciseExists_CallsDeleteOnRepository()
    {
        var exercise = Exercise.New(1, 1, "Test", "Content");
        var command = new DeleteExerciseCommand { Id = 1 };

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
        _exerciseRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercise);

        await _handler.Handle(command, CancellationToken.None);

        _exerciseRepositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsUnhandledExerciseException()
    {
        var exercise = Exercise.New(1, 1, "Test", "Content");
        var command = new DeleteExerciseCommand { Id = 1 };

        _exerciseQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Exercise>.Some(exercise));
        _exerciseRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UnhandledExerciseException>(ex));
    }
}