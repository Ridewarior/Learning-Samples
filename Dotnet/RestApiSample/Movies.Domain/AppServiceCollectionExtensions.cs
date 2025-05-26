using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Database;
using Movies.Domain.Repositories;
using Movies.Domain.Services;

namespace Movies.Domain;

public static class AppServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        services.AddSingleton<IMovieService, MovieService>();
        services.AddSingleton<IRatingRepository, RatingRepository>();
        services.AddValidatorsFromAssemblyContaining<IAppMarker>(ServiceLifetime.Singleton);
    }

    public static void AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));
        
        services.AddSingleton<DbInitializer>();
    }
}