
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UseCase.Contracts.Services;

namespace UseCase.Exceptions
{
    public class TournamentRoleHandler(IUserService userService, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<TournamentRoleRequirement>
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TournamentRoleRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                context.Fail();
                return;
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var routeData = httpContextAccessor.HttpContext?.GetRouteData();
            var tournamentIdValue = routeData?.Values["tournamentId"]?.ToString();
            if (!Guid.TryParse(tournamentIdValue, out var tournamentId))
            {
                context.Fail();
                return;
            }

            var userRole = await userService.GetUserRoleAndPermission(userId, tournamentId);

            if (userRole.Role.Equals(requirement.RequiredRole, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
