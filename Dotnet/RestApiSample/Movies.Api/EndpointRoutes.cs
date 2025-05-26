namespace Movies.Api;

public static class EndpointRoutes
{
    private const string root = "api";

    public static class Movies
    {
        private const string PathBase = $"{root}/movies";
        
        public const string GetAll = PathBase;
        public const string GetMovie = $"{PathBase}/{{idOrSlug}}";
        public const string CreateMovie = PathBase;
        public const string UpdateMovie = $"{PathBase}/{{id:guid}}";
        public const string DeleteMovie = $"{PathBase}/{{id:guid}}";

        public const string Rate = $"{PathBase}/{{id:guid}}/ratings";
        public const string DeleteRating = $"{PathBase}/{{id:guid}}/ratings";
    }

    public static class Ratings
    {
        private const string PathBase = $"{root}/ratings";

        public const string GetUserRatings = $"{PathBase}/me";
    }
}