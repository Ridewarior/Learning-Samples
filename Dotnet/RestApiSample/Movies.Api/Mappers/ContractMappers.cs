using Movies.Contracts.Requests;
using Movies.Domain.Models;

namespace Movies.Api.Mappers;

public static class ContractMappers
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };
    }
}