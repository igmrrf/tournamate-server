
using Domain.Entities;
using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;

namespace UseCase.Commands.TournamentCommand
{
    public class StartTournament
    {
        public record StartTournamentCommand(Guid tournamentId) : IRequest;

        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork) : IRequestHandler<StartTournamentCommand>
        {
            public async Task Handle(StartTournamentCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentDetail(t => t.Id == request.tournamentId) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                if(getTournament.TournamentInfo.StartDate <= DateTime.UtcNow)
                {
                    getTournament.StartTournament();
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                throw new UseCaseException($"Not yet Time To Start.",
                    "NotYetTime", (int)HttpStatusCode.BadRequest);
            }
        }

    }
}
