using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Grpc;
using Grpc.Core;
using GrpcServer.Models.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GrpcServer.GrpcServices;

public sealed class AuthService : Auth.AuthBase
{
    private readonly JwtOptions _jwtOptions;

    public AuthService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        if (request is { Login: "test", Password: "test" })
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, request.Login),
                new(ClaimTypes.Role, "Admin")
            };
            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.Lifetime)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), 
                    SecurityAlgorithms.HmacSha256));
            return Task.FromResult(new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken)
            });
        }

        throw new RpcException(new Status(StatusCode.Unauthenticated, "Неверное имя пользователя или пароль!"));
    }
}