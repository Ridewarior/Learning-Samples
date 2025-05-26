﻿using Movies.Domain.Models;

namespace Movies.Domain.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default);

    Task<Movie?> GetByIdAsync(Guid id, Guid? userId, CancellationToken cancellationToken = default);
   
    Task<Movie?> GetBySlugAsync(string slug, Guid? userId, CancellationToken cancellationToken = default);
   
    Task<IEnumerable<Movie>> GetAllAsync(Guid? userId, CancellationToken cancellationToken = default);
   
    Task<Movie?> UpdateAsync(Movie movie, Guid? userId, CancellationToken cancellationToken = default);
   
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}