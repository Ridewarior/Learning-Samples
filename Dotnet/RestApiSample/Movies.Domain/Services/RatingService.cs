using FluentValidation;
using FluentValidation.Results;
using Movies.Domain.Models;
using Movies.Domain.Repositories;

namespace Movies.Domain.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;

    public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken cToken = default)
    {
        if (rating is <= 0 or > 5)
        {
            throw new ValidationException([
                new ValidationFailure
                {
                    PropertyName = "Rating",
                    ErrorMessage = "Rating must be between 1 and 5"
                }
            ]);
        }

        var movieExists = await _movieRepository.ExistsByIdAsync(movieId, cToken);
        if (!movieExists)
        {
            return false;
        }

        return await _ratingRepository.RateMovieAsync(movieId, rating, userId, cToken);
    }
    
    public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken cToken = default)
    {
        return await _ratingRepository.DeleteRatingAsync(movieId, userId, cToken);
    }
    
    public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync (Guid userId, CancellationToken cToken = default)
    {
        return await _ratingRepository.GetRatingsForUserAsync(userId, cToken);
    }
}