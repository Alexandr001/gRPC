using System.Text;
using GrpcServer.AuthorizationPolicy;
using GrpcServer.AuthScheme;
using GrpcServer.Models.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace GrpcServer.Extension;

public static class AuthExtension
{
    public static AuthenticationBuilder AddJwt(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        var jwtOptions =
            configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new ArgumentNullException(nameof(JwtOptions));

        builder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
            };
        });
        return builder;
    }

    public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeyAuthSchemeOptions.SchemeName, options =>
        {
            options.Key = configuration.GetSection("ApiKey").Value;
        });
        return builder;
    }
}