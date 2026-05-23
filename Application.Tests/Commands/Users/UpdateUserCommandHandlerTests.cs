using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.Users;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IUserQueries> _userQueriesMock = new();
    private readonly Mock<IRoleQueries> _roleQueriesMock = new();
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _userQueriesMock.Object,
            _roleQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFoundException()
    {
        var command = CreateValidCommand();

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UserNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenEmailTakenByAnotherUser_ReturnsUserAlreadyExistsException()
    {
        var command = CreateValidCommand();
        var currentUser = CreateUserWithId("Roman", "roman@gmail.com", "hash", 1, 1);
        var anotherUser = CreateUserWithId("Other", "other@gmail.com", "hash", 1, 2);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(currentUser));
        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(anotherUser));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UserAlreadyExistsException>(ex));
    }

    [Fact]
    public async Task Handle_WhenRoleDoesNotExist_ReturnsRoleNotFoundException()
    {
        var command = CreateValidCommand();
        var currentUser = User.New("Roman", "roman@gmail.com", "hash", 1);

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(currentUser));
        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);
        _roleQueriesMock
            .Setup(x => x.GetByIdAsync(command.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Role>.None);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<RoleNotFoundException>(ex));
    }

    [Fact]
    public async Task Handle_WhenValid_ReturnsUpdatedUser()
    {
        var command = CreateValidCommand();
        var currentUser = User.New("Roman", "roman@gmail.com", "hash", 1);
        var role = Role.New("Admin");

        _userQueriesMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(currentUser));
        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);
        _roleQueriesMock
            .Setup(x => x.GetByIdAsync(command.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Role>.Some(role));
        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        _userRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static UpdateUserCommand CreateValidCommand() => new()
    {
        Id = 1,
        Name = "Roman Updated",
        Email = "roman@gmail.com",
        RoleId = 2
    };

    private static User CreateUserWithId(string name, string email, string hash, int roleId, int id)
    {
        var user = User.New(name, email, hash, roleId);
        typeof(User)
            .GetProperty("Id")!
            .SetValue(user, id);
        return user;
    }
}