using Application.Users.Commands.Auth;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Users;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var command = CreateValidCommand(email: "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = CreateValidCommand(email: "invalid");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var command = CreateValidCommand(password: "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static LoginUserCommand CreateValidCommand(
        string email = "roman@gmail.com",
        string password = "password123") => new()
        {
            Email = email,
            Password = password
        };
}