using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Repositories;

namespace Movies.Domain;

public static class AppServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
    }
}