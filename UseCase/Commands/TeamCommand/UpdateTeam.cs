

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using UseCase.Services;

namespace UseCase.Commands.TeamCommand
{
    public class UpdateTeam
    {
        public record UpdateTeamCommand(
            Guid TeamId,
            string? Name,
            string? Logo
        ) : IRequest;

        public class UpdateTeamCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<UpdateTeamCommand>
        {
            public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentTeamsAsync(tournament => tournament.Teams.Any(team => team.Id == request.TeamId)) ??
                throw new UseCaseException($"Team Not Found.",
               "NotFound", (int)HttpStatusCode.NotFound);

                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                if(!getTournament.Teams.Any(m => m.UserId == user.Id))
                {
                    throw new UseCaseException($"You are not authorized to update this team.",
                        "Unauthorized", (int)HttpStatusCode.BadRequest);
                }

                getTournament.UpdateTeam(request.TeamId, request.Name, request.Logo);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await Task.CompletedTask;
            }
        }
    }
}
