using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<Level> Levels { get; init; }
    public DbSet<Exercise> Exercises { get; init; }
    public DbSet<ExerciseType> ExerciseTypes { get; init; }
    public DbSet<Question> Questions { get; init; }
    public DbSet<AnswerOption> AnswerOptions { get; init; }
    public DbSet<PlacementTest> PlacementTests { get; init; }
    public DbSet<UserExerciseResult> UserExerciseResults { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasAnnotation("Relational:MigrationHistoryTable", "__efmigrationshistory");
        base.OnModelCreating(modelBuilder);
    }
}