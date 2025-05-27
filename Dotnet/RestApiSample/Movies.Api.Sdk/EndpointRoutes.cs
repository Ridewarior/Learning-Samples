namespace Movies.Api.Sdk;

public static class EndpointRoutes
{
    private const string root = "/api";

    public static class Movies
    {
        private const string PathBase = $"{root}/movies";
        
        public const string GetAll = PathBase;
        public const string GetMovie = $"{PathBase}/{{idOrSlug}}";
        public const string CreateMovie = PathBase;
        public const string UpdateMovie = $"{PathBase}/{{id}}";
        public const string DeleteMovie = $"{PathBase}/{{id}}";

        public const string Rate = $"{PathBase}/{{id}}/ratings";
        public const string DeleteRating = $"{PathBase}/{{id}}/ratings";
    }

    public static class Ratings
    {
        private const string PathBase = $"{root}/ratings";

        public const string GetUserRatings = $"{PathBase}/me";
    }
}