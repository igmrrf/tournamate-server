

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.TournamentCommand
{
    public class DeleteTournament
    {
        public record DeleteTournamentCommand(Guid TournamentId) : IRequest;

        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<DeleteTournamentCommand>
        {
            public async Task Handle(DeleteTournamentCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetAsync( t => t.Id == command.TournamentId
                                    && t.UserId == user.Id) ?? 
                    throw new UseCaseException($"Tournament Not Found.", 
                    "NotFound", (int)HttpStatusCode.NotFound);

                await tournamentRepository.DeleteAsync(getTournament);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}