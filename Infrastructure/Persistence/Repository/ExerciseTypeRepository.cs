using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ExerciseTypeRepository : BaseRepository<ExerciseType>, IRepository<ExerciseType>, IExerciseTypeQueries
{
    public ExerciseTypeRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Option<ExerciseType>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.ExerciseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity ?? Option<ExerciseType>.None;
    }

    public async Task<ExerciseType> CreateAsync(ExerciseType entity, CancellationToken cancellationToken)
    {
        await _context.ExerciseTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ExerciseType> UpdateAsync(ExerciseType entity, CancellationToken cancellationToken)
    {
        _context.ExerciseTypes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ExerciseType> DeleteAsync(ExerciseType entity, CancellationToken cancellationToken)
    {
        _context.ExerciseTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}