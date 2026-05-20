using Api.Filters;
using FluentValidation;

namespace Api.Modules;

public static class SetupModule
{
    public static void SetupServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        services.AddCors();
        services.AddRequestValidation();
    }

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy("AllowFrontend", policy =>
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));
    }

    private static void AddRequestValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
    }
}