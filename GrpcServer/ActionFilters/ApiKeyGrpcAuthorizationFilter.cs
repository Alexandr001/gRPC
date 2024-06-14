using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrpcServer.ActionFilters;

public sealed class ApiKeyGrpcAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var contextHttpContext = context.HttpContext;
        throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthorize"));
    }
}