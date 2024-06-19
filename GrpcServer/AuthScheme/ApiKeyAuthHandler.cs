using System.Security.Claims;
using System.Text.Encodings.Web;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace GrpcServer.AuthScheme;

public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
    public ApiKeyAuthHandler(
        IOptionsMonitor<ApiKeyAuthSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var key = Context.Request.Headers[ApiKeyAuthSchemeOptions.HeaderName];
        return Task.FromResult(Options.Key == key.ToString()
            ? AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), ApiKeyAuthSchemeOptions.SchemeName))
            : AuthenticateResult.Fail(new RpcException(new Status(StatusCode.Unauthenticated, ""))));
    }
}