using Grpc.Core;
using GrpcServer.AuthorizationPolicy;
using GrpcServer.AuthScheme;
using GrpcServer.Extension;
using GrpcServer.GrpcServices;
using GrpcServer.Interceptors;
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
        // serviceCollection
        //     .AddAuthentication(options => { options.DefaultScheme = ApiKeyAuthSchemeOptions.Name; })
        //     .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeyAuthSchemeOptions.Name, options => {});
        // serviceCollection.AddAuthorization(options =>
        // {
        //     options.AddPolicy(ApiKeyGrpcRequirement.NAME, builder => { builder.AddRequirements(new ApiKeyGrpcRequirement()); });
        // });
        serviceCollection.AddJwtToken(_configuration);
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
        // builder.Use( async (context, func) =>
        // {
        //     await func();
        //     int.TryParse(ProgramExtension.GRPC_PORT, out var val);
        //     if (context.Connection.LocalPort == val)
        //     {
        //         if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        //         {
        //             throw new RpcException(new Status(StatusCode.Unauthenticated, "Не авторизован!"));
        //         }
        //     }
        // });

        builder.UseSwagger();
        builder.UseSwaggerUI();

        builder.UseAuthentication();
        builder.UseAuthorization();

        builder.UseEndpoints(routeBuilder =>
        {
            routeBuilder.MapGrpcReflectionService();
            routeBuilder.MapGrpcService<ProductService>();
            routeBuilder.MapControllers();
        });
    }
}