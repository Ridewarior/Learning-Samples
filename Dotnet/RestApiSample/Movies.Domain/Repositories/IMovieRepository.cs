using Movies.Domain.Models;

namespace Movies.Domain.Repositories;

public interface IMovieRepository
{
   Task<bool> CreateAsync(Movie movie);

   Task<Movie?> GetByIdAsync(Guid id);
   
   Task<Movie?> GetBySlugAsync(string slug);
   
   Task<IEnumerable<Movie>> GetAllAsync();
   
   Task<bool> UpdateAsync(Movie movie);
   
   Task<bool> DeleteAsync(Guid id);
}