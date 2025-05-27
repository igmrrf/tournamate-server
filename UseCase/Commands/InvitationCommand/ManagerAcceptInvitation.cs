

using System.Net;
using Domain.Aggregate.TournamentAggregate;
using Domain.Enum;
using Domain.Shared.Enum;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.InvitationCommand
{
    public class ManagerAcceptInvitation
    {
        public record ManagerAcceptInviteCOmmand : IRequest
        {
            public string TeamLogo { get; set; }
            public string TeamName { get; set; }
            public Guid TournamentId { get; set; }
            public string Code { get; set; }
        }

        public class Handler(ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<ManagerAcceptInviteCOmmand>
        {
            public async Task Handle(ManagerAcceptInviteCOmmand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentDetail(t => t.Id == request.TournamentId
                && t.InvitationCode == request.Code) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);


                if( getTournament.Status != TournamentStatus.Upcoming)
                {
                    throw new UseCaseException($"Tournament Is Not Yet Publish.",
                    "NotYetPublish", (int)HttpStatusCode.NotFound);
                }

                if(getTournament.IsTournamentStarted)
                {
                    throw new UseCaseException($"Tournament Already Started.",
                    "AlreadyStarted", (int)HttpStatusCode.NotFound);
                }

                if (getTournament.TournamentMode == TournamentMode.PlayerVsPlayer)
                {
                    throw new UseCaseException($"Tournament Is Not A Team VS Team.",
                    "NotApplied", (int)HttpStatusCode.NotFound);
                }


                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var team = new Team(request.TeamName, request.TeamLogo, user.Id, request.TournamentId, 
                    getTournament.NoOfPlayers, getTournament.NoOfSubPlayers, request.Code);

                getTournament.AddTeam(team);
                getTournament.AddParticipant(user.Id, Role.Manager);

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
