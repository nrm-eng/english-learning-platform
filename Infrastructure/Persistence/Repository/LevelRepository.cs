using Application.Common.Interfaces.Queries;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository;

public class LevelRepository(ApplicationDbContext context)
    : BaseRepository<Level>(context), ILevelQueries
{
}