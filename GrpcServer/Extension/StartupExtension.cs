using GrpcServer.AuthScheme;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GrpcServer.Extension;

public static class StartupExtension
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwt(configuration)
            .AddApiKey(configuration);
        
        serviceCollection.AddAuthorizationBuilder()
                    .AddPolicy(JwtBearerDefaults.AuthenticationScheme, AuthorizationExtension.JwtPolicy)
                    .AddPolicy(ApiKeyAuthSchemeOptions.SchemeName, AuthorizationExtension.ApiKeyPolicy)
                    .SetDefaultPolicy(AuthorizationExtension.ApiKeyPolicy);
    }
}