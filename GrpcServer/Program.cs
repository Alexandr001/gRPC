using GrpcServer;
using GrpcServer.Extension;
using Microsoft.AspNetCore.Server.Kestrel.Core;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder
        .UseStartup<Startup>()
        .ConfigureKestrel(options =>
        {
            options.ListenPortByOptions(ProgramExtension.HTTP_PORT, HttpProtocols.Http1);
            options.ListenPortByOptions(ProgramExtension.GRPC_PORT, HttpProtocols.Http2);
        }))
    .Build()
    .RunAsync();