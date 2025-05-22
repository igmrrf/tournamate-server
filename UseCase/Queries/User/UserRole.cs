

using MediatR;
using UseCase.Contracts.Services;
using UseCase.DTOs;

namespace UseCase.Queries.User
{
    public class UserRole
    {
        public record UserRoleCommand(Guid TournamentId) : IRequest<UserRoleAndPermission>;
        
        public class Handler(IUserService userService) : IRequestHandler<UserRoleCommand, UserRoleAndPermission>
        {
            public async Task<UserRoleAndPermission> Handle(UserRoleCommand request, CancellationToken cancellationToken)
            {
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");
                var userRole = await userService.GetUserRoleAndPermission(request.TournamentId, user.Id);
                return new UserRoleAndPermission
                {
                    CanMakeSubstitutions = userRole.CanMakeSubstitutions,
                    CanRecordFoul = userRole.CanRecordFoul,
                    CanUpdateScore = userRole.CanUpdateScore,
                    CanUpdateTimeStamp = userRole.CanUpdateTimeStamp,
                    Role = userRole.Role
                };
            }
        }

    }
}
