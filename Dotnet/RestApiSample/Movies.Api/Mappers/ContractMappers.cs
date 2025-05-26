using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
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

    public static MovieResponse MapToMovieResponse(this Movie movie)
    {
        return new()
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            Rating = movie.Rating,
            UserRating = movie.UserRating,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres
        };
    }

    public static MoviesResponse MapToMoviesResponse(this IEnumerable<Movie> movies)
    {
        return new()
        {
            Movies = movies.Select(movie => movie.MapToMovieResponse())
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        return new()
        {
            Id = id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };
    }

    public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
    {
        return ratings.Select(x => new MovieRatingResponse
        {
            MovieId = x.MovieId,
            Rating = x.Rating,
            Slug = x.Slug,
        });
    }
}