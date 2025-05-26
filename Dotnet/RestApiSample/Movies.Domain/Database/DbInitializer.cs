using Dapper;

namespace Movies.Domain.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        // primary table schema
        await connection.ExecuteAsync("""
                                      create table if not exists movies (
                                         id UUID primary key,
                                         slug TEXT not null,
                                         title TEXT not null,
                                         yearofrelease integer not null
                                      );
                                      """);

        // primary table slug indexing
        await connection.ExecuteAsync("""
                                      create unique index concurrently if not exists movies_slug_idx on movies
                                      using btree(slug)
                                      """);

        // create genre table
        await connection.ExecuteAsync("""
                                      create table if not exists genres (
                                          movieId UUID references movies (Id),
                                          name TEXT not null);
                                      """);

        // ratings
        await connection.ExecuteAsync("""
                                      create table if not exists ratings (
                                          userid UUID,
                                          movieid UUID references movies (id),
                                          rating integer not null,
                                          primary key (userid, movieid)
                                      );
                                      """);
    }
}