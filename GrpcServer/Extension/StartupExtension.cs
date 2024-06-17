using System.Text;
using GrpcServer.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GrpcServer.Extension;

public static class StartupExtension
{
    public static void AddJwtToken(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var jwtOptions =
            configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() 
            ?? throw new ArgumentNullException(nameof(JwtOptions));

        serviceCollection.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
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
    }
}