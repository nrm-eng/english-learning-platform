using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Users.Commands;

public class CreateUserCommand : IRequest<Either<BaseException, User>>
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class CreateUserCommandHandler(
    IRepository<User> userRepository,
    IUserQueries userQueries,
    IRoleQueries roleQueries) : IRequestHandler<CreateUserCommand, Either<BaseException, User>>
{
    public async Task<Either<BaseException, User>> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userQueries.GetByEmailAsync(request.Email, cancellationToken);
        return await existingUser.MatchAsync(
            u => new UserAlreadyExistsException(u.Email),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, User>> CreateEntity(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var isFirstUser = !(await userQueries.GetAllAsync(cancellationToken)).Any();
            var roleName = isFirstUser ? "Admin" : "User";

            var role = await roleQueries.GetByNameAsync(roleName, cancellationToken);
            var roleId = role.Match(
                r => r.Id,
                () => throw new RoleNotFoundException(0));

            var user = await userRepository.CreateAsync(
                User.New(request.Name, request.Email, passwordHash, roleId),
                cancellationToken);
            return user;
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(0, ex);
        }
    }
}