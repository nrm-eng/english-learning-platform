using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Roles.Commands;

public class DeleteRoleCommand : IRequest<Either<BaseException, Role>>
{
    public required int Id { get; init; }
}

public class DeleteRoleCommandHandler(
    IRepository<Role> roleRepository,
    IRoleQueries roleQueries,
    IUserQueries userQueries) : IRequestHandler<DeleteRoleCommand, Either<BaseException, Role>>
{
    public async Task<Either<BaseException, Role>> Handle(
        DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(request.Id, cancellationToken);
        return await role.MatchAsync(
            r => CheckDependencies(r.Id, cancellationToken)
                .BindAsync(_ => DeleteEntity(r, cancellationToken)),
            () => new RoleNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        int roleId, CancellationToken cancellationToken)
    {
        var users = await userQueries.GetByRoleIdAsync(roleId, cancellationToken);
        return users.Any()
            ? new RoleHasUsersException(roleId)
            : LanguageExt.Unit.Default;
    }

    private async Task<Either<BaseException, Role>> DeleteEntity(
        Role role, CancellationToken cancellationToken)
    {
        try
        {
            return await roleRepository.DeleteAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledRoleException(role.Id, ex);
        }
    }
}