using Microsoft.AspNetCore.Mvc;

namespace GrpcServer.ActionFilters;

public class ApiKeyGrpcAttribute : ServiceFilterAttribute<ApiKeyGrpcAuthorizationFilter>;