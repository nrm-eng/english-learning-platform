using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Commands;
using Domain.Entities;
using LanguageExt;
using Moq;

namespace Application.Tests.Commands.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IUserQueries> _userQueriesMock = new();
    private readonly Mock<IRoleQueries> _roleQueriesMock = new();
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _userQueriesMock.Object,
            _roleQueriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsUserAlreadyExistsException()
    {
        var command = CreateValidCommand();
        var existingUser = CreateUser();

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.Some(existingUser));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UserAlreadyExistsException>(ex));
    }

    [Fact]
    public async Task Handle_WhenFirstUser_AssignsAdminRole()
    {
        var command = CreateValidCommand();
        var adminRole = CreateRole("Admin", 2);

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);
        _userQueriesMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());
        _roleQueriesMock
            .Setup(x => x.GetByNameAsync("Admin", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Role>.Some(adminRole));
        _userRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken _) => u);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        result.IfRight(user => Assert.Equal(2, user.RoleId));
    }

    [Fact]
    public async Task Handle_WhenNotFirstUser_AssignsUserRole()
    {
        var command = CreateValidCommand();
        var userRole = CreateRole("User", 1);
        var existingUsers = new List<User> { CreateUser() };

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);
        _userQueriesMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUsers);
        _roleQueriesMock
            .Setup(x => x.GetByNameAsync("User", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Role>.Some(userRole));
        _userRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken _) => u);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsRight);
        result.IfRight(user => Assert.Equal(1, user.RoleId));
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsUnhandledUserException()
    {
        var command = CreateValidCommand();
        var userRole = CreateRole("User", 1);

        _userQueriesMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);
        _userQueriesMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { CreateUser() });
        _roleQueriesMock
            .Setup(x => x.GetByNameAsync("User", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<Role>.Some(userRole));
        _userRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsLeft);
        result.IfLeft(ex => Assert.IsType<UnhandledUserException>(ex));
    }

    private static CreateUserCommand CreateValidCommand() => new()
    {
        Name = "Roman",
        Email = "roman@gmail.com",
        Password = "password123"
    };

    private static User CreateUser() => User.New("Roman", "roman@gmail.com", "hashedpassword", 1);
    private static Role CreateRole(string name, int id)
    {
        var role = Role.New(name);
        typeof(Role)
            .GetProperty("Id")!
            .SetValue(role, id);
        return role;
    }
}