using Grpc;
using GrpcServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrpcServer.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly UserService _service;

    public TestController(UserService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<Product[]> GetAllUserAsync(CancellationToken cancellationToken)
    {
        var userAsync = await _service.GetUserAsync(cancellationToken);
        return userAsync;
    }
}