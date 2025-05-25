

using System.Net;
using Domain.Aggregate.TournamentAggregate;
using Domain.Entities;
using Domain.Shared.Enum;
using MediatR;
using UseCase.Contracts.InternalServices;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.TournamentCommand
{
    public class PublishTournament
    {
        public record PublishTournamentCommand(Guid TournamentId, string CallbackUrl) : IRequest;
        

        public class Handler(ITournamentRepository tournamentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IUserService userService, IEmailProvider emailProvider) : IRequestHandler<PublishTournamentCommand>
        {
            public async Task Handle(PublishTournamentCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetTournamentWithParticipantAsync( t => t.Id == command.TournamentId
                                    && t.UserId == user.Id) ?? 
                    throw new UseCaseException($"Tournament Not Found.", 
                    "NotFound", (int)HttpStatusCode.NotFound);


                if (getTournament.Participants.Count >= 1)
                {
                    var referees = getTournament.Participants
                        .Where(p => p.Role == Role.Referee)
                        .ToList();

                    var notificationTasks = new List<Task>();
                    foreach (var referee in referees)
                    {
                        // Get user details
                        var userRef = await userRepository.GetAsync(u => u.Id == referee.UserId);
                        if (userRef == null || string.IsNullOrWhiteSpace(userRef.Email))
                        {
                            continue; // Skip if no valid email
                        }

                        // Format permissions
                        var permissionsDescription = DescribePermissions(referee.Permissions);


                        notificationTasks.Add(emailProvider.TournamentRefereeNotificationEmail(userRef.UserName, userRef.Email, getTournament.TournamentInfo.Name,
                            getTournament.TournamentInfo.StartDate, permissionsDescription, command.CallbackUrl));
                    }
                }

                getTournament.PublishTournament(getTournament);

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            private string DescribePermissions(IEnumerable<Permission> permissions)
            {
                if (permissions == null || !permissions.Any())
                    return "No specific permissions granted";

                var permissionList = new List<string>();

                foreach (var permission in permissions)
                {
                    var permissionsForUser = new List<string>();

                    if (permission.CanUpdateScore)
                        permissionsForUser.Add("Update match scores");

                    if (permission.CanRecordFoul)
                        permissionsForUser.Add("Record fouls");

                    if (permission.CanMakeSubstitutions)
                        permissionsForUser.Add("Manage substitutions");

                    if (permission.CanUpdateTimeStamp)
                        permissionsForUser.Add("Update match timing");

                    if (permissionsForUser.Any())
                    {
                        permissionList.Add($"- {string.Join(", ", permissionsForUser)}");
                    }
                }

                return permissionList.Any()
                    ? string.Join(Environment.NewLine, permissionList)
                    : "Basic referee access (no special permissions)";
            }
        }
    }
}