using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.Contracts.Requests;
using Movies.Domain.Services;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet(EndpointRoutes.Movies.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken cToken)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions()
            .WithUser(userId);

        var movies = await _movieService.GetAllAsync(options, cToken);
        var movieCount = await _movieService.GetCountAsync(options.Title, options.YearOfRelease, cToken);

        var response = movies.MapToMoviesResponse(request.Page, request.PageSize, movieCount);
        return Ok(response);
    }

    [HttpGet(EndpointRoutes.Movies.GetMovie)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken cToken)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, userId, cToken)
            : await _movieService.GetBySlugAsync(idOrSlug, userId, cToken);

        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.TrustedMemberPolicy)]
    [HttpPost(EndpointRoutes.Movies.CreateMovie)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken cToken)
    {
        var movie = request.MapToMovie();
        var wasCreated = await _movieService.CreateAsync(movie, cToken);

        if (!wasCreated)
        {
            return Problem(statusCode: 500, detail: "Failed to create movie");
        }

        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [Authorize(AuthConstants.TrustedMemberPolicy)]
    [HttpPut(EndpointRoutes.Movies.UpdateMovie)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
        CancellationToken cToken)
    {
        var userId = HttpContext.GetUserId();
        var movie = request.MapToMovie(id);

        var wasUpdated = await _movieService.UpdateAsync(movie, userId, cToken);
        if (wasUpdated is null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicy)]
    [HttpDelete(EndpointRoutes.Movies.DeleteMovie)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cToken)
    {
        var wasDeleted = await _movieService.DeleteAsync(id, cToken);
        if (!wasDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}