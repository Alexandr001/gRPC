using Microsoft.AspNetCore.Mvc;

namespace GrpcServer.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public Task<string> IsActiveAsync()
    {
        return Task.FromResult("I'm active");
    }
}