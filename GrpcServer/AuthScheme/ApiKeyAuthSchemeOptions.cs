using Microsoft.AspNetCore.Authentication;

namespace GrpcServer.AuthScheme;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public const string SchemeName = "ApiKey";
    public const string HeaderName = "X-API-KEY";

    public string? Key { get; set; }
}