﻿using System.Text.RegularExpressions;

namespace Movies.Domain.Models;

public partial class Movie
{
    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
    
    public required Guid Id { get; init; }

    public required string Title { get; set; }

    public required int YearOfRelease { get; set; }

    public string Slug => GenerateSlug();

    public required List<string> Genres { get; init; } = [];
    
    public float? Rating { get; set; }
    
    public int? UserRating { get; set; }
    
    private string GenerateSlug()
    {
        var slugTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");

        return $"{slugTitle}-{YearOfRelease}";
    }
}