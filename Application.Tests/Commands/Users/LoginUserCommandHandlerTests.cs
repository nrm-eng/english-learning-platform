using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Services;
using Application.Users.Commands.Auth;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.Users;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserQueries> _userQueriesMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _handler = new LoginUserCommandHandler(
            _userQueriesMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnsUserNotFoundException()
    {
        var command = CreateValidCommand();

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UserNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenPasswordIsWrong_ReturnsUserNotFoundException()
    {
        var command = CreateValidCommand();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        var user = User.New("Roman", "roman@gmail.com", passwordHash, 1);

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UserNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenCredentialsValid_ReturnsToken()
    {
        var command = CreateValidCommand();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
        var user = User.New("Roman", "roman@gmail.com", passwordHash, 1);
        var expectedToken = "jwt.token.here";

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(user));
        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(expectedToken);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        result.IfRight(token => Assert.Equal(expectedToken, token));
    }

    private static LoginUserCommand CreateValidCommand() => new()
    {
        Email = "roman@gmail.com",
        Password = "wrongpassword"
    };
}