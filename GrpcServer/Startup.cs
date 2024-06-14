using GrpcServer.ActionFilters;
using GrpcServer.GrpcServices;
using GrpcServer.Interceptors;

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
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddGrpc(options => { options.Interceptors.Add<ExceptionInterceptor>(); });
        serviceCollection.AddGrpcReflection();
        serviceCollection.AddSwaggerGen();
        
        serviceCollection.AddSingleton<ApiKeyGrpcAuthorizationFilter>();
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
            routeBuilder.MapControllers();
        });
    }
}