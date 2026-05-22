using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<RoleRepository>();
        services.AddScoped<LevelRepository>();
        services.AddScoped<ExerciseTypeRepository>();
        services.AddScoped<ExerciseRepository>();
        services.AddScoped<QuestionRepository>();
        services.AddScoped<AnswerOptionRepository>();
        services.AddScoped<PlacementTestRepository>();
        services.AddScoped<UserExerciseResultRepository>();

        services.AddScoped<IRepository<User>>(p => p.GetRequiredService<UserRepository>());
        services.AddScoped<IRepository<Role>>(p => p.GetRequiredService<RoleRepository>());
        services.AddScoped<IRepository<ExerciseType>>(p => p.GetRequiredService<ExerciseTypeRepository>());
        services.AddScoped<IRepository<Exercise>>(p => p.GetRequiredService<ExerciseRepository>());
        services.AddScoped<IRepository<Question>>(p => p.GetRequiredService<QuestionRepository>());
        services.AddScoped<IRepository<AnswerOption>>(p => p.GetRequiredService<AnswerOptionRepository>());
        services.AddScoped<IRepository<PlacementTest>>(p => p.GetRequiredService<PlacementTestRepository>());
        services.AddScoped<IRepository<UserExerciseResult>>(p => p.GetRequiredService<UserExerciseResultRepository>());

        services.AddScoped<IUserQueries>(p => p.GetRequiredService<UserRepository>());
        services.AddScoped<IRoleQueries>(p => p.GetRequiredService<RoleRepository>());
        services.AddScoped<ILevelQueries>(p => p.GetRequiredService<LevelRepository>());
        services.AddScoped<IExerciseTypeQueries>(p => p.GetRequiredService<ExerciseTypeRepository>());
        services.AddScoped<IExerciseQueries>(p => p.GetRequiredService<ExerciseRepository>());
        services.AddScoped<IQuestionQueries>(p => p.GetRequiredService<QuestionRepository>());
        services.AddScoped<IAnswerOptionQueries>(p => p.GetRequiredService<AnswerOptionRepository>());
        services.AddScoped<IPlacementTestQueries>(p => p.GetRequiredService<PlacementTestRepository>());
        services.AddScoped<IUserExerciseResultQueries>(p => p.GetRequiredService<UserExerciseResultRepository>());
    }
}