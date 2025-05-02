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

    [HttpGet(EndpointRoutes.Movies.PathBase)]
    public async Task<IActionResult> GetAll()
    {
       var movies = await _movieRepository.GetAllAsync();
       
       var response = movies.MapToMoviesResponse();
       return Ok(response);
    }

    [HttpGet(EndpointRoutes.Movies.GetMovie)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie is null)
        {
            return NotFound();
        }
        
        var response = movie.MapToMovieResponse();
        return Ok(response);
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

        var response = movie.MapToMovieResponse();
        return Created($"{EndpointRoutes.Movies.PathBase}/{movie.Id}", response);

    }
}