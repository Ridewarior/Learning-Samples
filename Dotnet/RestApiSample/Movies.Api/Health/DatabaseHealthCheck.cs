using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Domain.Database;

namespace Movies.Api.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory, ILogger<DatabaseHealthCheck> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            _ = await _dbConnectionFactory.CreateConnectionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database is unhealthy");
            return HealthCheckResult.Unhealthy();
        }
        
        return HealthCheckResult.Healthy();
    }
}