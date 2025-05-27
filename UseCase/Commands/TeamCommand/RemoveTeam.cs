

using MediatR;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using UseCase.Services;

namespace UseCase.Commands.TeamCommand
{
    public class RemoveTeam
    {
        public record RemoveTeamCommand(Guid TeamId) : IRequest;

        public class RemoveTeamCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<RemoveTeamCommand>
        {
            public async Task Handle(RemoveTeamCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentTeamsAsync(tournament => tournament.Teams.Any(team => team.Id == request.TeamId)) ??
                throw new UseCaseException($"Team Not Found.",
               "NotFound", (int)HttpStatusCode.NotFound);

                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                if (getTournament.UserId != user.Id)
                {
                    throw new UseCaseException($"You are not authorized to update this team.",
                        "Unauthorized", (int)HttpStatusCode.BadRequest);
                }

                getTournament.RemoveTeam(request.TeamId);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await Task.CompletedTask;
            }
        }
    }
}
