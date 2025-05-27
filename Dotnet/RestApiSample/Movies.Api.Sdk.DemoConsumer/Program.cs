using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Api.Sdk.DemoConsumer;
using Movies.Contracts.Requests;
using Refit;

// var moviesApi = RestService.For<IMoviesApi>("https://localhost:5001");

var services = new ServiceCollection();
services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(x => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (_, _) => await x.GetRequiredService<AuthTokenProvider>()
            .GetTokenAsync()
    })
    .ConfigureHttpClient(x =>
        x.BaseAddress = new Uri("https://localhost:5001"));

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movie = await moviesApi.GetMovieAsync("the-matrix");
var movies = await moviesApi.GetMoviesAsync(new GetAllMoviesRequest
{
    Page = 1,
    PageSize = 3
});

Console.WriteLine(JsonSerializer.Serialize(movie));
Console.WriteLine(JsonSerializer.Serialize(movies));
