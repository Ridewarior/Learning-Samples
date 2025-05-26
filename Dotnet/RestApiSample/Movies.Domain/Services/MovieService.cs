using FluentValidation;
using Movies.Domain.Models;
using Movies.Domain.Repositories;

namespace Movies.Domain.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _validator;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> validator)
    {
        _movieRepository = movieRepository;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        await _validator.ValidateAndThrowAsync(movie);
        return await _movieRepository.CreateAsync(movie);
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        return await _movieRepository.GetByIdAsync(id);
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        return await _movieRepository.GetBySlugAsync(slug);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _movieRepository.GetAllAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        await _validator.ValidateAndThrowAsync(movie);
        var movieFound = await _movieRepository.ExistsByIdAsync(movie.Id);
        if (!movieFound)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie);
        return movie;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _movieRepository.DeleteAsync(id);
    }
}