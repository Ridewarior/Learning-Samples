using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Refit;

namespace Movies.Api.Sdk;

[Headers("Authorization: Bearer")]
public interface IMoviesApi
{
    [Get(EndpointRoutes.Movies.GetMovie)]
    Task<MovieResponse> GetMovieAsync(string idOrSlug);

    [Get(EndpointRoutes.Movies.GetAll)]
    Task<MoviesResponse> GetMoviesAsync(GetAllMoviesRequest request);

    [Post(EndpointRoutes.Movies.CreateMovie)]
    Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);
    
    [Put(EndpointRoutes.Movies.UpdateMovie)]
    Task<MovieResponse> UpdateMovieAsync(string id, UpdateMovieRequest request);
    
    [Delete(EndpointRoutes.Movies.DeleteMovie)]
    Task DeleteMovieAsync(string id);
    
    [Post(EndpointRoutes.Movies.Rate)]
    Task RateMovieAsync(string id, RateMovieRequest request);
    
    [Delete(EndpointRoutes.Movies.DeleteRating)]
    Task DeleteRatingAsync(string id);
    
    [Get(EndpointRoutes.Ratings.GetUserRatings)]
    Task<IEnumerable<MovieRatingResponse>> GetUserRatingsAsync();
}