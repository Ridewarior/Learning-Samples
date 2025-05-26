using FluentValidation;
using Movies.Domain.Models;
using Movies.Domain.Repositories;

namespace Movies.Domain.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IValidator<GetAllMoviesOptions> _getAllMoviesOptionsValidator;

    public MovieService(IMovieRepository movieRepository,
        IValidator<Movie> movieValidator,
        IRatingRepository ratingRepository, IValidator<GetAllMoviesOptions> getAllMoviesOptionsValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
        _ratingRepository = ratingRepository;
        _getAllMoviesOptionsValidator = getAllMoviesOptionsValidator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);
        return await _movieRepository.CreateAsync(movie, cancellationToken);
    }

    public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId, CancellationToken cancellationToken = default)
    {
        return await _movieRepository.GetByIdAsync(id, userId, cancellationToken);
    }

    public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId, CancellationToken cancellationToken = default)
    {
        return await _movieRepository.GetBySlugAsync(slug, userId, cancellationToken);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken cancellationToken = default)
    {
        await _getAllMoviesOptionsValidator.ValidateAndThrowAsync(options, cancellationToken);
        
        return await _movieRepository.GetAllAsync(options, cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);
        var movieFound = await _movieRepository.ExistsByIdAsync(movie.Id, cancellationToken);
        if (!movieFound)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie, cancellationToken);

        if (userId.HasValue)
        {
            var rating = await _ratingRepository.GetRatingAsync(movie.Id, userId.Value, cancellationToken);
            movie.Rating = rating.Rating;
            movie.UserRating = rating.UserRating;
        }
        else
        {
            var rating = await _ratingRepository.GetRatingAsync(movie.Id, cancellationToken);
            movie.Rating = rating;
        }
        
        return movie;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _movieRepository.DeleteAsync(id, cancellationToken);
    }
}