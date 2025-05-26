using Dapper;
using Movies.Domain.Database;
using Movies.Domain.Models;

namespace Movies.Domain.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken cToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cToken);

        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
            select round(avg(r.rating), 1)
            from ratings r
            where movieid = @movieId
            """, new { movieId }, cancellationToken: cToken));
    }

    public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId,
        CancellationToken cToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cToken);

        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
            select round(avg(r.rating), 1),
                   (
                       select rating
                       from ratings
                       where movieid = @movieId
                       and userid = @userId
                       limit 1
                   )
            from ratings r
            where movieid = @movieId
            """, new { movieId, userId }, cancellationToken: cToken));
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken cToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cToken);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         insert into ratings (userid, movieid, rating)
                                                                         values (@userId, @movieId, @rating)
                                                                         on conflict (userid, movieid) do update set rating = @rating
                                                                         """, new { userId, movieId, rating },
            cancellationToken: cToken));

        return result > 0;
    }

    public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken cToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cToken);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         delete from ratings 
                                                                            where userid = @userId 
                                                                              and movieid = @movieId
                                                                         """, new { userId, movieId },
            cancellationToken: cToken));

        return result > 0;
    }

    public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken cToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cToken);
        return await connection.QueryAsync<MovieRating>(new CommandDefinition("""
                                                                              select m.id, m.slug, r.rating
                                                                              from ratings r
                                                                              inner join movies m on r.movieid = m.id
                                                                              where r.userid = @userId
                                                                              """, new { userId },
            cancellationToken: cToken));
    }
}