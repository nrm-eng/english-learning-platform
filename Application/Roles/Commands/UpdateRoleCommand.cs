using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Roles.Commands;

public class UpdateRoleCommand : IRequest<Either<BaseException, Role>>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateRoleCommandHandler(
    IRepository<Role> roleRepository,
    IRoleQueries roleQueries) : IRequestHandler<UpdateRoleCommand, Either<BaseException, Role>>
{
    public async Task<Either<BaseException, Role>> Handle(
        UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(request.Id, cancellationToken);
        return await role.MatchAsync(
            r => CheckDuplicates(r.Id, request.Name, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, r, cancellationToken)),
            () => new RoleNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Role>> UpdateEntity(
        UpdateRoleCommand request, Role role, CancellationToken cancellationToken)
    {
        try
        {
            role.UpdateDetails(request.Name);
            return await roleRepository.UpdateAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledRoleException(role.Id, ex);
        }
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDuplicates(
        int currentRoleId, string name, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByNameAsync(name, cancellationToken);
        return role.Match<Either<BaseException, LanguageExt.Unit>>(
            r => r.Id == currentRoleId
                ? LanguageExt.Unit.Default
                : new RoleAlreadyExistsException(name),
            () => LanguageExt.Unit.Default);
    }
}