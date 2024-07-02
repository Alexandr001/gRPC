using System.Net;
using Grpc.Core;

namespace GrpcServer.HttpHandlers;

public class GrpcDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<GrpcDelegatingHandler> _logger;

    public GrpcDelegatingHandler(ILogger<GrpcDelegatingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request start");

        var message = await base.SendAsync(request, cancellationToken);
        if (message.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException(
                httpRequestError: HttpRequestError.UserAuthenticationError,
                statusCode: HttpStatusCode.Unauthorized);
        }

        return message;
    }
}