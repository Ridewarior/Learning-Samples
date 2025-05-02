using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Repositories;

namespace Movies.Domain;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}