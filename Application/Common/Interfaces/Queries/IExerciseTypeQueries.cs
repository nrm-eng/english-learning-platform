using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IExerciseTypeQueries : IBaseQuery<ExerciseType>
{
    Task<Option<ExerciseType>> GetByNameAsync(string name, CancellationToken cancellationToken);
}