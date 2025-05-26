using Movies.Domain.Models;

namespace Movies.Domain.Repositories;

public interface IRatingRepository
{
   Task<float?> GetRatingAsync (Guid movieId, CancellationToken cToken = default);

   Task<(float? Rating, int? UserRating)> GetRatingAsync (Guid movieId, Guid userId, CancellationToken cToken = default);

   Task<bool> RateMovieAsync (Guid movieId, int rating, Guid userId, CancellationToken cToken = default);
   
   Task<bool> DeleteRatingAsync (Guid movieId, Guid userId, CancellationToken cToken = default);
   
   Task<IEnumerable<MovieRating>> GetRatingsForUserAsync (Guid userId, CancellationToken cToken = default);
}