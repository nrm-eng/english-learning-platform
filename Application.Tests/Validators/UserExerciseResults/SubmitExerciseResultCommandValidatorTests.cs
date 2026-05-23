using Application.UserExerciseResults.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.UserExerciseResults;

public class SubmitExerciseResultCommandValidatorTests
{
    private readonly SubmitExerciseResultCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Invalid()
    {
        var command = CreateValidCommand(userId: 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Have_Error_When_ExerciseId_Is_Invalid()
    {
        var command = CreateValidCommand(exerciseId: 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ExerciseId);
    }

    [Fact]
    public void Should_Have_Error_When_Score_Exceeds_MaxScore()
    {
        var command = CreateValidCommand(score: 10, maxScore: 5);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Score);
    }

    [Fact]
    public void Should_Have_Error_When_MaxScore_Is_Zero()
    {
        var command = CreateValidCommand(maxScore: 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.MaxScore);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static SubmitExerciseResultCommand CreateValidCommand(
        int userId = 1,
        int exerciseId = 1,
        int score = 1,
        int maxScore = 1) => new()
        {
            UserId = userId,
            ExerciseId = exerciseId,
            Score = score,
            MaxScore = maxScore
        };
}