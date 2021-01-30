using AuthUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Authorization
{
    public class PermissionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionsMiddleware> _logger;

        public PermissionsMiddleware(
            RequestDelegate next,
            ILogger<PermissionsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context, IUserPermissionService permissionService)
        {
            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var cancellationToken = context.RequestAborted;

            var userSub = context.User.FindFirst(StandardJwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrEmpty(userSub))
            {
                await context.WriteAccessDeniedResponse("User 'sub' claim is required", cancellationToken: cancellationToken);
                return;
            }

            var permissionsIdentity = await permissionService.GetUserPermissionsIdentity(userSub, cancellationToken);
            if (permissionsIdentity == null)
            {
                _logger.LogWarning("User {sub} does not have permissions", userSub);

                await context.WriteAccessDeniedResponse(cancellationToken: cancellationToken);
                return;
            }

            // User has permissions, so we add the extra identity containing the "permissions" claims
            context.User.AddIdentity(permissionsIdentity);
            await _next(context);
        }
    }
}