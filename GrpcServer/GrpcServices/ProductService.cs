using Google.Protobuf.WellKnownTypes;
using Grpc;
using Grpc.Core;
using GrpcServer.AuthorizationPolicy;
using GrpcServer.AuthScheme;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace GrpcServer.GrpcServices;

public sealed class ProductService : Products.ProductsBase
{
    [Authorize]
    public override Task<GetProductsResponse> GetAllProducts(GetProductsRequest request, ServerCallContext context)
    {
        var getProductsResponse = new GetProductsResponse();
        var products = new List<Product>()
        {
            new()
            {
                Id = 10,
                Name = "New",
                Description = "Desc",
                DataTimeCreate = Timestamp.FromDateTime(DateTime.UtcNow)
            }
        };
        getProductsResponse.Products.AddRange(products);
        return Task.FromResult(getProductsResponse);
    }

    [Authorize(Policy = JwtBearerDefaults.AuthenticationScheme)]
    public override Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request,
        ServerCallContext context)
    {
        return Task.FromResult(new GetProductByIdResponse
        {
            Product = new Product
            {
                Id = 1,
                Name = "Name",
                DataTimeCreate = Timestamp.FromDateTime(DateTime.UtcNow),
                Description = "Desc"
            }
        });
    }

    public override Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        return base.CreateProduct(request, context);
    }

    public override Task<DeleteProductByIdResponse> DeleteProductById(DeleteProductByIdRequest request,
        ServerCallContext context)
    {
        return base.DeleteProductById(request, context);
    }
}