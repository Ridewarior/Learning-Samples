using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappers;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Movies.Domain.Repositories;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost(EndpointRoutes.Movies.PathBase)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        var wasCreated = await _movieRepository.CreateAsync(movie);

        if (!wasCreated)
        {
            return Problem(statusCode: 500, detail: "Failed to create movie");
        }
        
        MovieResponse response = new()
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres
        };

        return Created($"{EndpointRoutes.Movies.PathBase}/{movie.Id}", response);

    }
}