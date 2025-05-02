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

    [HttpGet(EndpointRoutes.Movies.GetAll)]
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

    [HttpPost(EndpointRoutes.Movies.CreateMovie)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        var wasCreated = await _movieRepository.CreateAsync(movie);

        if (!wasCreated)
        {
            return Problem(statusCode: 500, detail: "Failed to create movie");
        }

        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new {id = movie.Id}, response);
    }

    [HttpPut(EndpointRoutes.Movies.UpdateMovie)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        
        var wasUpdated = await _movieRepository.UpdateAsync(movie);
        if (!wasUpdated)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [HttpDelete(EndpointRoutes.Movies.DeleteMovie)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var wasDeleted = await _movieRepository.DeleteAsync(id);
        if (!wasDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}