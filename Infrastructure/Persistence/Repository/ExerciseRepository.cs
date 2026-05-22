using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ExerciseRepository : BaseRepository<Exercise>, IRepository<Exercise>, IExerciseQueries
{
    public ExerciseRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public override async Task<IReadOnlyList<Exercise>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .Include(x => x.Level)
            .Include(x => x.Type)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public override async Task<Option<Exercise>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises
            .Include(x => x.Level)
            .Include(x => x.Type)
            .Include(x => x.Questions)!
            .ThenInclude(x => x.AnswerOptions)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<Exercise>.None;
    }

    public async Task<IReadOnlyList<Exercise>> GetByLevelIdAsync(int levelId, CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .Include(x => x.Level)
            .Include(x => x.Type)
            .AsNoTracking()
            .Where(x => x.LevelId == levelId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Exercise>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .Include(x => x.Level)
            .Include(x => x.Type)
            .AsNoTracking()
            .Where(x => x.TypeId == typeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Exercise>> GetByLevelAndTypeAsync(
        int levelId,
        int typeId,
        CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .Include(x => x.Level)
            .Include(x => x.Type)
            .AsNoTracking()
            .Where(x => x.LevelId == levelId && x.TypeId == typeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Exercise> CreateAsync(Exercise entity, CancellationToken cancellationToken)
    {
        await _context.Exercises.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Exercise> UpdateAsync(Exercise entity, CancellationToken cancellationToken)
    {
        _context.Exercises.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Exercise> DeleteAsync(Exercise entity, CancellationToken cancellationToken)
    {
        _context.Exercises.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}