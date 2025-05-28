using Minimap.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PeopleService>();
builder.Services.AddSingleton<GuidGenerator>();

var app = builder.Build();

app.MapGet("get-example", () => "Hello from GET method");
app.MapPost("post-example", () => "Hello from POST method");

app.MapGet("get-params/{age:int}", (int age) => $"Hello from GET method with age: {age}");
app.MapGet("cars/{carid:regex(^[a-z0-9]+$)}", (string carid) => $"Hello from GET method with carid: {carid}");
app.MapGet("book/{isbn:length(13)", (string isbn) => $"ISBN: {isbn}");

app.MapGet("people/search", (string? searchTerm, PeopleService peopleService) =>
{
    if (searchTerm is null)
    {
        return Results.NotFound();
    }

    var results = peopleService.Search(searchTerm);
    return Results.Ok(results);
});

app.MapGet("mix/{routeParam}",
    (string routeParam, int queryParam, GuidGenerator guidGenerator) =>
    {
        return $"{routeParam}, {queryParam}, {guidGenerator.NewGuid}";
    });

app.MapPost("people", (Person person) =>
{
    return Results.Ok(person);
});

app.Run();