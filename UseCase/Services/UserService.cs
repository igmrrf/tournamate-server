using Domain.Entities;
using Domain.Shared.Enum;
using Domain.ValueObject;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.DTOs;
using UseCase.Exceptions;

namespace UseCase.Services
{
    public class UserService(IHttpContextAccessor accessor, IUserRepository userRepository, IRoleRepository roleRepository, ITournamentRepository tournamentRepository, IPermissionRepository permissionRepository) : IUserService
    {
        public async Task<User?> LoggedInUser()
        {
            try
            {
                var userId = accessor?.HttpContext?.User?.Claims?.FirstOrDefault(u => u.Type == "Id")?.Value;
                var getUser = await userRepository.GetAsync(u => u.Id.ToString() == userId);
                return getUser;
            }
            catch (UseCaseException ex)
            {
                throw new UseCaseException($"User Not Verified",
                            "NotVerified", (int)HttpStatusCode.NotFound);
            }
        }

        public async Task<UserRoleAndPermission> GetUserRoleAndPermission(Guid userId, Guid tournamentId)
        {
            var tournament = await tournamentRepository.GetAsync(t => t.Id == tournamentId)
                ?? throw new UseCaseException("Tournament not found", "NotFound", (int)HttpStatusCode.NotFound);

            var roles = await roleRepository.GetListAsync(x => x.UserId == userId && x.TournamentId == tournamentId);

            var permission = await permissionRepository.GetAsync(x => x.UserId == userId && x.TournamentId == tournamentId);

            var isReferee = permission is not null;
            var isAdmin = roles.Any(r => r.Role == Role.Admin);
            var isManager = roles.Any(r => r.Role == Role.Manager);

            Role resolvedRole;

            if (tournament.IsTournamentStarted && isReferee)
            {
                resolvedRole = Role.Referee;
            }
            else if (isAdmin)
            {
                resolvedRole = Role.Admin;
            }
            else if (isManager)
            {
                resolvedRole = Role.Manager;
            }
            else if (isReferee)
            {
                resolvedRole = Role.Referee;
            }
            else
            {
                resolvedRole = Role.Spectator;
            }

            return new UserRoleAndPermission
            {
                Role = resolvedRole.ToString(),
                CanMakeSubstitutions = permission?.CanMakeSubstitutions ?? false,
                CanRecordFoul = permission?.CanRecordFoul ?? false,
                CanUpdateScore = permission?.CanUpdateScore ?? false,
                CanUpdateTimeStamp = permission?.CanUpdateTimeStamp ?? false
            };
        }

    }
}
