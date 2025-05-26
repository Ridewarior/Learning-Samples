using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Database;
using Movies.Domain.Repositories;

namespace Movies.Domain;

public static class AppServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
    }

    public static void AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));
        
        services.AddSingleton<DbInitializer>();
    }
}