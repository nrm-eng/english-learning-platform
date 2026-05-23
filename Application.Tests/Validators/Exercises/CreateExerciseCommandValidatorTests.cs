using Application.Exercises.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Exercises;

public class CreateExerciseCommandValidatorTests
{
    private readonly CreateExerciseCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_LevelId_Is_Invalid()
    {
        var command = CreateValidCommand(levelId: 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LevelId);
    }

    [Fact]
    public void Should_Have_Error_When_TypeId_Is_Invalid()
    {
        var command = CreateValidCommand(typeId: 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TypeId);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var command = CreateValidCommand(title: "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var command = CreateValidCommand(content: "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateExerciseCommand CreateValidCommand(
        int levelId = 1,
        int typeId = 1,
        string title = "Test Exercise",
        string content = "Test Content") => new()
        {
            LevelId = levelId,
            TypeId = typeId,
            Title = title,
            Content = content
        };
}