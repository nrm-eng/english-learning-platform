using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository;

public class AnswerOptionRepository : BaseRepository<AnswerOption>, IRepository<AnswerOption>, IAnswerOptionQueries
{
    public AnswerOptionRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<AnswerOption> CreateAsync(AnswerOption entity, CancellationToken cancellationToken)
    {
        await _context.AnswerOptions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<AnswerOption> UpdateAsync(AnswerOption entity, CancellationToken cancellationToken)
    {
        _context.AnswerOptions.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<AnswerOption> DeleteAsync(AnswerOption entity, CancellationToken cancellationToken)
    {
        _context.AnswerOptions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}