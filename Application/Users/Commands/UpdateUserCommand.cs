using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserCommand : IRequest<Either<BaseException, User>>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required int RoleId { get; init; }
}

public class UpdateUserCommandHandler(
    IRepository<User> userRepository,
    IUserQueries userQueries,
    IRoleQueries roleQueries) : IRequestHandler<UpdateUserCommand, Either<BaseException, User>>
{
    public async Task<Either<BaseException, User>> Handle(
        UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByIdAsync(request.Id, cancellationToken);
        return await user.MatchAsync(
            u => CheckDuplicates(u.Id, request.Email, cancellationToken)
                .BindAsync(_ => CheckDependencies(request.RoleId, cancellationToken)
                    .BindAsync(_ => UpdateEntity(request, u, cancellationToken))),
            () => new UserNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, User>> UpdateEntity(
        UpdateUserCommand request, User user, CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateProfile(request.Name, request.Email);
            user.ChangeRole(request.RoleId);
            return await userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(user.Id, ex);
        }
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        int roleId, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(roleId, cancellationToken);
        return role.IsNone
            ? new RoleNotFoundException(roleId)
            : LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDuplicates(
        int currentUserId, string email, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByEmailAsync(email, cancellationToken);
        return user.Match<Either<BaseException, LanguageExt.Unit>>(
            u => u.Id == currentUserId
                ? LanguageExt.Unit.Default
                : new UserAlreadyExistsException(email),
            () => LanguageExt.Unit.Default);
    }
}