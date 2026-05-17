using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IPlacementTestQueries : IBaseQuery<PlacementTest>
{
    Task<Option<PlacementTest>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
}