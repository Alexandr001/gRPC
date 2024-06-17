using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace GrpcServer.AuthorizationPolicy;

public sealed class ApiKeyGrpcHandler : AuthorizationHandler<ApiKeyGrpcRequirement>
{
    private readonly ILogger<ApiKeyGrpcHandler> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    public ApiKeyGrpcHandler(ILogger<ApiKeyGrpcHandler> logger, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyGrpcRequirement requirement)
    {
        var key = _contextAccessor.HttpContext?.Request.Headers["X-API-KEY"] ?? StringValues.Empty;
        if (!StringValues.IsNullOrEmpty(key))
        {
            _logger.LogInformation(key.ToString());
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        
        return Task.CompletedTask;
    }
}