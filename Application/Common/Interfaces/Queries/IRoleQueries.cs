using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IRoleQueries : IBaseQuery<Role>
{
    Task<Option<Role>> GetByNameAsync(string name, CancellationToken cancellationToken);
}