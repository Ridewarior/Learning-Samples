using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappers;
using Movies.Contracts.Requests;
using Movies.Domain.Services;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet(EndpointRoutes.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieService.GetAllAsync();

        var response = movies.MapToMoviesResponse();
        return Ok(response);
    }

    [HttpGet(EndpointRoutes.Movies.GetMovie)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id)
            : await _movieService.GetBySlugAsync(idOrSlug);

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
        var wasCreated = await _movieService.CreateAsync(movie);

        if (!wasCreated)
        {
            return Problem(statusCode: 500, detail: "Failed to create movie");
        }

        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpPut(EndpointRoutes.Movies.UpdateMovie)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);

        var wasUpdated = await _movieService.UpdateAsync(movie);
        if (wasUpdated is null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [HttpDelete(EndpointRoutes.Movies.DeleteMovie)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var wasDeleted = await _movieService.DeleteAsync(id);
        if (!wasDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}