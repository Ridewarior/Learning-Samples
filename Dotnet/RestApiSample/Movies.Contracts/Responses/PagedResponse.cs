﻿namespace Movies.Contracts.Responses;

public class PagedResponse<TResponse>
{
    public required IEnumerable<TResponse> Items { get; init; } = [];
    
    public required int PageSize { get; init; }
    
    public required int Page { get; init; }
    
    public required int TotalPages { get; init; }

    public bool HasNextPage => TotalPages > (Page * PageSize);
}