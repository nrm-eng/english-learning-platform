using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.ExerciseTypes.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.ExerciseTypes;

public class DeleteExerciseTypeCommandHandlerTests
{
    private readonly Mock<IRepository<ExerciseType>> _exerciseTypeRepositoryMock = new();
    private readonly Mock<IExerciseTypeQueries> _exerciseTypeQueriesMock = new();
    private readonly Mock<IExerciseQueries> _exerciseQueriesMock = new();
    private readonly DeleteExerciseTypeCommandHandler _handler;

    public DeleteExerciseTypeCommandHandlerTests()
    {
        _handler = new DeleteExerciseTypeCommandHandler(
            _exerciseTypeRepositoryMock.Object,
            _exerciseTypeQueriesMock.Object,
            _exerciseQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenExerciseTypeHasExercises_ReturnsExerciseTypeHasExercisesException()
    {
        var command = new DeleteExerciseTypeCommand { Id = 1 };
        var exerciseType = ExerciseType.New("Reading");
        var exercises = new List<Exercise> { Exercise.New(1, 1, "Test", "Content") };

        _exerciseTypeQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<ExerciseType>.Some(exerciseType));

        _exerciseQueriesMock
            .Setup(x => x.GetByTypeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercises);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<ExerciseTypeHasExercisesException>(ex));
    }

    [Fact]
    public async Task Handle_WhenValid_ReturnsDeletedExerciseType()
    {
        var command = new DeleteExerciseTypeCommand { Id = 1 };
        var exerciseType = ExerciseType.New("Reading");

        _exerciseTypeQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<ExerciseType>.Some(exerciseType));

        _exerciseQueriesMock
            .Setup(x => x.GetByTypeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Exercise>());

        _exerciseTypeRepositoryMock
            .Setup(x => x.DeleteAsync(exerciseType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exerciseType);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
    }
}