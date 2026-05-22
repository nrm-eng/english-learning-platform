using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class RoleRepository(ApplicationDbContext context)
    : BaseRepository<Role>(context), IRepository<Role>, IRoleQueries
{
    public async Task<Option<Role>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        return entity ?? Option<Role>.None;
    }

    public async Task<Role> CreateAsync(Role entity, CancellationToken cancellationToken)
    {
        await _context.Roles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken)
    {
        _context.Roles.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken)
    {
        _context.Roles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
}