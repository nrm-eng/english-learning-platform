using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class UserExerciseResultRepository : BaseRepository<UserExerciseResult>, IRepository<UserExerciseResult>, IUserExerciseResultQueries
{
    public UserExerciseResultRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Option<UserExerciseResult>> GetByUserAndExerciseAsync(
        int userId,
        int exerciseId,
        CancellationToken cancellationToken)
    {
        var entity = await _context.UserExerciseResults
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.UserId == userId && x.ExerciseId == exerciseId,
                cancellationToken);

        return entity ?? Option<UserExerciseResult>.None;
    }

    public async Task<IReadOnlyList<UserExerciseResult>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        return await _context.UserExerciseResults
            .Include(x => x.Exercise)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserExerciseResult> CreateAsync(
        UserExerciseResult entity,
        CancellationToken cancellationToken)
    {
        await _context.UserExerciseResults.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<UserExerciseResult> UpdateAsync(
        UserExerciseResult entity,
        CancellationToken cancellationToken)
    {
        _context.UserExerciseResults.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<UserExerciseResult> DeleteAsync(
        UserExerciseResult entity,
        CancellationToken cancellationToken)
    {
        _context.UserExerciseResults.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}