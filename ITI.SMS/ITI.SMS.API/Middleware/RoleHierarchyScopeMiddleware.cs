using System.Net;
using System.Text.Json;
using ITI.SMS.API.Controllers;
using ITI.SMS.Application.Common.Interfaces;

namespace ITI.SMS.API.Middleware;

public class RoleHierarchyScopeMiddleware
{
    private readonly RequestDelegate _next;

    public RoleHierarchyScopeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IRoleHierarchyScopeService scopeService)
    {
        // Only run for authenticated users
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // The JWT stores roles in an array, so we might have multiple role claims
            var roles = context.User.FindAll(System.Security.Claims.ClaimTypes.Role)
                                    .Select(c => c.Value)
                                    .ToArray();

            if (!string.IsNullOrEmpty(userId))
            {
                int? branchId = GetRouteValueAsInt(context, "branchId");
                int? trackId = GetRouteValueAsInt(context, "trackId");
                int? courseId = GetRouteValueAsInt(context, "courseId");

                // Only check if at least one scope parameter is present
                if (branchId.HasValue || trackId.HasValue || courseId.HasValue)
                {
                    bool hasAccess = await scopeService.HasAccessAsync(userId, roles, branchId, trackId, courseId);
                    
                    if (!hasAccess)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";

                        var response = new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Forbidden",
                            Data = null
                        };

                        // Because ApiResponse doesn't match the exact error envelope in requirements directly in its generic form,
                        // we'll serialize a custom object matching the required envelope:
                        // { "success": false, "error": { "code": "FORBIDDEN", "details": ["You do not have access to this resource."] } }
                        
                        var errorResponse = new 
                        {
                            success = false,
                            error = new 
                            {
                                code = "FORBIDDEN",
                                details = new[] { "You do not have access to this resource." }
                            }
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                        return;
                    }
                }
            }
        }

        await _next(context);
    }

    private static int? GetRouteValueAsInt(HttpContext context, string key)
    {
        if (context.Request.RouteValues.TryGetValue(key, out var value) && value != null)
        {
            if (int.TryParse(value.ToString(), out int result))
            {
                return result;
            }
        }
        return null;
    }
}
