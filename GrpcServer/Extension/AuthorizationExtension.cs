using GrpcServer.AuthScheme;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace GrpcServer.Extension;

public static class AuthorizationExtension
{
    public static Microsoft.AspNetCore.Authorization.AuthorizationPolicy JwtPolicy => 
        new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    
    public static Microsoft.AspNetCore.Authorization.AuthorizationPolicy ApiKeyPolicy =>
        new AuthorizationPolicyBuilder(ApiKeyAuthSchemeOptions.SchemeName)
            .AddAuthenticationSchemes(ApiKeyAuthSchemeOptions.SchemeName)
            .RequireAssertion(context =>
            {
                var isAuthenticated = context.User.Identity?.IsAuthenticated ?? true;
                return isAuthenticated;
            })
            .Build();
}