

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.TournamentCommand
{
    public class PublishTournament
    {
        public record PublishTournamentCommand(Guid TournamentId) : IRequest;
        

        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<PublishTournamentCommand>
        {
            public async Task Handle(PublishTournamentCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetAsync( t => t.Id == command.TournamentId
                                    && t.UserId == user.Id) ?? 
                    throw new UseCaseException($"Tournament Not Found.", 
                    "NotFound", (int)HttpStatusCode.NotFound);


                getTournament.PublishTournament(getTournament);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}