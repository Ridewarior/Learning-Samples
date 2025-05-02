namespace Movies.Api;

public static class EndpointRoutes
{
    private const string root = "api";

    public static class Movies
    {
        public const string PathBase = $"{root}/movies";
        public const string GetMovie = $"{PathBase}/{{id:guid}}";
    }
}