using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcServer.Extension;

public static class ProgramExtension
{
    public const string HTTP_PORT = "HTTP_PORT";
    public const string GRPC_PORT = "GRPC_PORT";
    
    public static void ListenPortByOptions(
        this KestrelServerOptions options, 
        string envOpt,
        HttpProtocols httpProtocols)
    {
        var isHttpPortParsed = int.TryParse(Environment.GetEnvironmentVariable(envOpt), out var httpPort);

        if (isHttpPortParsed)
        {
            options.Listen(IPAddress.Any, httpPort, listenOptions =>
            {
                listenOptions.Protocols = httpProtocols;
            });
        }
    }
}