using Grpc.Core;
using GrpcServer.AuthorizationPolicy;
using GrpcServer.AuthScheme;
using GrpcServer.Extension;
using GrpcServer.GrpcServices;
using GrpcServer.Interceptors;
using GrpcServer.Models.Options;
using Microsoft.AspNetCore.Authorization;

namespace GrpcServer;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.Configure<JwtOptions>(_configuration.GetSection(JwtOptions.SectionName));

        serviceCollection.AddAuth(_configuration);
        
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();

        serviceCollection.AddGrpc(options => { options.Interceptors.Add<ExceptionInterceptor>(); });
        serviceCollection.AddGrpcReflection();

        serviceCollection.AddSwaggerGen();
        serviceCollection.AddHttpContextAccessor();

        serviceCollection.AddScoped<IAuthorizationHandler, ApiKeyGrpcHandler>();
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseRouting();
        
        builder.UseSwagger();
        builder.UseSwaggerUI();

        builder.UseAuthentication();
        builder.UseAuthorization();

        builder.UseEndpoints(routeBuilder =>
        {
            routeBuilder.MapGrpcReflectionService();
            routeBuilder.MapGrpcService<ProductService>();
            routeBuilder.MapGrpcService<AuthService>();
            routeBuilder.MapControllers();
        });
    }
}