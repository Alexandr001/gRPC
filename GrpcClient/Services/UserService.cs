using Grpc;
using Grpc.Core;

namespace GrpcServer.Services;

public class UserService
{
    private readonly Products.ProductsClient _client;

    public UserService(Products.ProductsClient client)
    {
        _client = client;
    }

    public async Task<Product[]> GetUserAsync(CancellationToken cancellationToken)
    {
        var response = await _client.GetAllProductsAsync(
            new GetProductsRequest(), 
            cancellationToken: cancellationToken, 
            headers: [
                // new Metadata.Entry("X-API-KEY", "keykey")
            ]).ResponseAsync;
        return response.Products.ToArray();
    }
}