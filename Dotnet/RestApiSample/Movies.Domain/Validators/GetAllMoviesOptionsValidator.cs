using FluentValidation;
using Movies.Domain.Models;

namespace Movies.Domain.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
   private static readonly string[] _acceptableSortFields = ["title", "yearofrelease"];
   
   public GetAllMoviesOptionsValidator()
   {
      RuleFor(x => x.YearOfRelease)
         .LessThanOrEqualTo(DateTime.UtcNow.Year);

      RuleFor(x => x.SortField)
         .Must(x => x is null || _acceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
         .WithMessage("You can only sort by 'title' or 'yearofrelease'");
   } 
}