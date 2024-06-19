using System.Text;
using GrpcServer.AuthorizationPolicy;
using GrpcServer.AuthScheme;
using GrpcServer.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace GrpcServer.Extension;

public static class StartupExtension
{
    public static void AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwt(configuration)
            .AddApiKey(configuration);

        var apiKeyAuthorizePolicy = new AuthorizationPolicyBuilder(ApiKeyAuthSchemeOptions.SchemeName)
            .AddAuthenticationSchemes(ApiKeyAuthSchemeOptions.SchemeName)
            .RequireAssertion(context =>
            {
                var isAuthenticated = context.User.Identity?.IsAuthenticated ?? true;
                return isAuthenticated;
            })
            .Build();
        var jwtAuthorizePolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(
                JwtBearerDefaults.AuthenticationScheme,
                jwtAuthorizePolicy);
            options.AddPolicy(
                ApiKeyAuthSchemeOptions.SchemeName,
                apiKeyAuthorizePolicy);
            
            options.DefaultPolicy = apiKeyAuthorizePolicy;
        });
    }
}