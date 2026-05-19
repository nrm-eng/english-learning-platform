using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Roles.Commands;

public class CreateRoleCommand : IRequest<Either<BaseException, Role>>
{
    public required string Name { get; init; }
}

public class CreateRoleCommandHandler(
    IRepository<Role> roleRepository,
    IRoleQueries roleQueries) : IRequestHandler<CreateRoleCommand, Either<BaseException, Role>>
{
    public async Task<Either<BaseException, Role>> Handle(
        CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await roleQueries.GetByNameAsync(request.Name, cancellationToken);
        return await existingRole.MatchAsync(
            r => new RoleAlreadyExistsException(r.Name),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, Role>> CreateEntity(
        CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await roleRepository.CreateAsync(
                Role.New(request.Name),
                cancellationToken);
            return role;
        }
        catch (Exception ex)
        {
            return new UnhandledRoleException(0, ex);
        }
    }
}