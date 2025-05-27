using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Movies.Api.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _config;

    public ApiKeyAuthFilter(IConfiguration config)
    {
        _config = config;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedObjectResult("API key is missing");
            return;
        }
        
        var settingKey = _config["ApiKey"]!;
        if (settingKey != apiKey)
        {
            context.Result = new UnauthorizedObjectResult("API key is invalid");
        }
    }
}