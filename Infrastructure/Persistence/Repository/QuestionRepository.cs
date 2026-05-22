using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class QuestionRepository : BaseRepository<Question>, IRepository<Question>, IQuestionQueries
{
    public QuestionRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public override async Task<Option<Question>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Questions
            .Include(x => x.AnswerOptions)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<Question>.None;
    }

    public async Task<Question> CreateAsync(Question entity, CancellationToken cancellationToken)
    {
        await _context.Questions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Question> UpdateAsync(Question entity, CancellationToken cancellationToken)
    {
        _context.Questions.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Question> DeleteAsync(Question entity, CancellationToken cancellationToken)
    {
        _context.Questions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}