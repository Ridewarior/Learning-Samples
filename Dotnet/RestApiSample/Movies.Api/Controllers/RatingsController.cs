using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.Contracts.Requests;
using Movies.Domain.Services;

namespace Movies.Api.Controllers;

[ApiController]
public class RatingsController : ControllerBase
{
   private readonly IRatingService _ratingService;

   public RatingsController(IRatingService ratingService)
   {
      _ratingService = ratingService;
   }
   
   [Authorize]
   [HttpPut(EndpointRoutes.Movies.Rate)]
   public async Task<IActionResult> RateMovie([FromRoute] Guid id,
      [FromBody] RateMovieRequest request,
      CancellationToken cToken)
   {
      var userId = HttpContext.GetUserId();
      var wasRated = await _ratingService.RateMovieAsync(id,
         request.Rating, 
         userId!.Value,
         cToken);
      
      return wasRated ? Ok() : NotFound();
   }

   [Authorize]
   [HttpDelete(EndpointRoutes.Movies.DeleteRating)]
   public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken cToken)
   {
      var userId = HttpContext.GetUserId();
      var wasDeleted = await _ratingService.DeleteRatingAsync(id, userId!.Value, cToken);
      
      return wasDeleted ? Ok() : NotFound();
   }

   [Authorize]
   [HttpGet(EndpointRoutes.Ratings.GetUserRatings)]
   public async Task<IActionResult> GetUserRatings(CancellationToken cToken)
   {
      var userId = HttpContext.GetUserId();
      var ratings = await _ratingService.GetRatingsForUserAsync(userId!.Value, cToken);
      var ratingsResponse = ratings.MapToResponse();

      return Ok(ratingsResponse);
   }
}