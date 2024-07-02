using System.Net;
using Grpc;
using GrpcServer.HttpHandlers;
using GrpcServer.Services;
using Microsoft.AspNetCore.Diagnostics;

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

        serviceCollection.AddScoped<UserService>();
        serviceCollection.AddTransient<GrpcDelegatingHandler>();

        serviceCollection.AddGrpcClient<Products.ProductsClient>(o => 
                o.Address = new Uri("http://localhost:5002"))
            .AddHttpMessageHandler<GrpcDelegatingHandler>();
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseExceptionHandler(applicationBuilder =>
        {
            applicationBuilder.Run( async context =>
            {
                
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature?.Error is HttpRequestException e)
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int) e.StatusCode!;
                        await context.Response.WriteAsync(e.Message);
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(contextFeature?.Error?.Message ?? "");
                    }
            });
        });
        builder.UseRouting();
        builder.UseEndpoints(routeBuilder => { routeBuilder.MapControllers(); });
    }
}