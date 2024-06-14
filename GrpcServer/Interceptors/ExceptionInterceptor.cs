using Grpc.Core;
using Grpc.Core.Interceptors;

namespace GrpcServer.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogWarning("AsyncUnaryCall");
        return base.AsyncUnaryCall(request, context, continuation);
    }

    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogWarning("BlockingUnaryCall");
        return base.BlockingUnaryCall(request, context, continuation);
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await base.UnaryServerHandler(request, context, continuation);
            return response;
        }
        catch (RpcException e)
        {
            _logger.LogError(e, "RpcException happened");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception happened");
            throw new RpcException(new Status(StatusCode.Internal, e.Message));
        }
    }
}