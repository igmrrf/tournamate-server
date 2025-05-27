

using MediatR;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using UseCase.Services;

namespace UseCase.Commands.TeamCommand
{
    public class RemovePlayerFromTeam
    {
        public record RemovePlayerTeamCommand(Guid TeamId, Guid PlayerId) : IRequest;

        public class RemoveTeamCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<RemovePlayerTeamCommand>
        {
            public async Task Handle(RemovePlayerTeamCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentTeamsAsync(tournament => tournament.Teams.Any(team => team.Id == request.TeamId)) ??
                throw new UseCaseException($"Team Not Found.",
               "NotFound", (int)HttpStatusCode.NotFound);

                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                if (!getTournament.Teams.Any(m => m.UserId == user.Id))
                {
                    throw new UseCaseException($"You are not authorized to Remove this Player.",
                        "Unauthorized", (int)HttpStatusCode.BadRequest);
                }

                getTournament.RemovePlayerFromTeam(request.TeamId, request.PlayerId);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await Task.CompletedTask;
            }

        }
    }
}
