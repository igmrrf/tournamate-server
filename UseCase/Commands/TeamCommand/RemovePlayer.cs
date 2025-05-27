

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using UseCase.Services;

namespace UseCase.Commands.TeamCommand
{
    public class RemovePlayer
    {
        public record RemovePlayerCommand(Guid PlayerId) : IRequest;

        public class RemovePlayerCommandHandler( ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<RemovePlayerCommand>
        {
            public async Task Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentPlayersAsync(tournament => tournament.Players.Any(player => player.Id == request.PlayerId)) ??
                throw new UseCaseException($"Player Not Found.",
               "NotFound", (int)HttpStatusCode.NotFound);

                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                if (getTournament.UserId != user.Id)
                {
                    throw new UseCaseException($"You are not authorized to Remove this Player.",
                        "Unauthorized", (int)HttpStatusCode.BadRequest);
                }

                getTournament.RemovePlayer(request.PlayerId);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await Task.CompletedTask;
            }
        }
    }
}
