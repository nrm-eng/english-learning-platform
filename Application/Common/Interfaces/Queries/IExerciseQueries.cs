using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IExerciseQueries : IBaseQuery<Exercise>
{
    Task<IReadOnlyList<Exercise>> GetByLevelIdAsync(int levelId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Exercise>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Exercise>> GetByLevelAndTypeAsync(int levelId, int typeId, CancellationToken cancellationToken);
}