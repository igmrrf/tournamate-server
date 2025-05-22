

using Microsoft.AspNetCore.Authorization;

namespace UseCase.Exceptions
{
    public class TournamentRoleRequirement : IAuthorizationRequirement
    {
        public string RequiredRole { get; }

        public TournamentRoleRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}
