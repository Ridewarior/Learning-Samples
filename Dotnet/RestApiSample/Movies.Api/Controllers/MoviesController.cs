using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Movies.Domain.Services;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IOutputCacheStore _outputCacheStore;

    public MoviesController(IMovieService movieService, IOutputCacheStore outputCacheStore)
    {
        _movieService = movieService;
        _outputCacheStore = outputCacheStore;
    }

    [HttpGet(EndpointRoutes.Movies.GetAll)]
    [OutputCache(PolicyName = "MovieCache")]
    // [ResponseCache(Duration = 30, VaryByQueryKeys = new[]{ "title", "yearofrelease", "sortby", "page", "pagesize"})]
    [ProducesResponseType<MoviesResponse>(StatusCodes.Status200OK)]
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
    [OutputCache]
    // [ResponseCache(Duration = 30, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType<MovieResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationFailureResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken cToken)
    {
        var movie = request.MapToMovie();
        var wasCreated = await _movieService.CreateAsync(movie, cToken);

        if (!wasCreated)
        {
            return Problem(statusCode: 500, detail: "Failed to create movie");
        }

        await _outputCacheStore.EvictByTagAsync("movies", cToken);

        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [Authorize(AuthConstants.TrustedMemberPolicy)]
    [HttpPut(EndpointRoutes.Movies.UpdateMovie)]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationFailureResponse>(StatusCodes.Status400BadRequest)]
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
        
        await _outputCacheStore.EvictByTagAsync("movies", cToken);
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicy)]
    [HttpDelete(EndpointRoutes.Movies.DeleteMovie)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cToken)
    {
        var wasDeleted = await _movieService.DeleteAsync(id, cToken);
        if (!wasDeleted)
        {
            return NotFound();
        }

        await _outputCacheStore.EvictByTagAsync("movies", cToken);
        return Ok();
    }
}