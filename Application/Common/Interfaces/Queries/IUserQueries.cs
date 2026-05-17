using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IUserQueries : IBaseQuery<User>
{
    Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken);
    Task<Option<User>> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken);
}