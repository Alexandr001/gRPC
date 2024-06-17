using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

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
        return Task.FromResult(!StringValues.IsNullOrEmpty(key) 
            ? AuthenticateResult.Fail("") 
            : AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), ApiKeyAuthSchemeOptions.Name)));
    }
}