using Microsoft.AspNetCore.Authentication;

namespace GrpcServer.AuthScheme;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "ApiKey";
    public const string HeaderName = "X-API-KEY";
}