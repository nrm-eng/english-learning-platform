using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class PlacementTestRepository : BaseRepository<PlacementTest>, IRepository<PlacementTest>, IPlacementTestQueries
{
    public PlacementTestRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Option<PlacementTest>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var entity = await _context.PlacementTests
            .Include(x => x.Level)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        return entity ?? Option<PlacementTest>.None;
    }

    public async Task<PlacementTest> CreateAsync(PlacementTest entity, CancellationToken cancellationToken)
    {
        await _context.PlacementTests.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<PlacementTest> UpdateAsync(PlacementTest entity, CancellationToken cancellationToken)
    {
        _context.PlacementTests.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<PlacementTest> DeleteAsync(PlacementTest entity, CancellationToken cancellationToken)
    {
        _context.PlacementTests.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}