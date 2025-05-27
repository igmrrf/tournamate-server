

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using UseCase.Queries.Tournament;
using UseCase.Services;

namespace UseCase.Commands.TeamCommand
{
    public class UpdatePlayer
    {
        public record UpdatePlayerCommand(
            Guid PlayerId,
            string? Name,
            string? Position,
            string? JerseyNumber
        ) : IRequest;

        public class UpdatePlayerCommandHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<UpdatePlayerCommand>
        {
            public async Task Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
            {
                var player = await tournamentRepository.GetTournamentPlayersAsync( p => p.Players.Any(ply => ply.Id == request.PlayerId))
                    ?? throw new UseCaseException($"Player Not Found.",
                "NotFound", (int)HttpStatusCode.NotFound);

                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                if (!player.Players.Any(p => p.Id == user.Id))
                {
                    throw new UseCaseException($"You are not authorized to update this Player.",
                        "Unauthorized", (int)HttpStatusCode.BadRequest);
                }

                player.UpdatePlayer(player.Id, request.Name, request.Position, request.JerseyNumber);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await Task.CompletedTask;
            }
        }
    }
}
