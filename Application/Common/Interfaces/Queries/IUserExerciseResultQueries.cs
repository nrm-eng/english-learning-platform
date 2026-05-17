using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IUserExerciseResultQueries : IBaseQuery<UserExerciseResult>
{
    Task<Option<UserExerciseResult>> GetByUserAndExerciseAsync(int userId, int exerciseId, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserExerciseResult>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
}