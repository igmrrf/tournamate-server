

using MediatR;
using static UseCase.Commands.TournamentCommand.SaveToDraft;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.TournamentCommand
{
    public class CancelTournament
    {
        public record CancelTournamentCommand(Guid TournamentId) : IRequest;
        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<CancelTournamentCommand>
        {
            public async Task Handle(CancelTournamentCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetAsync(t => t.Id == command.TournamentId
                                    && t.UserId == user.Id) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                getTournament.CancelTournament(getTournament);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
