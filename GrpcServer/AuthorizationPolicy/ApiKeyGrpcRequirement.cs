using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrpcServer.AuthorizationPolicy;

public class ApiKeyGrpcRequirement : IAuthorizationRequirement
{
    public const string NAME = "ApiKey";
}