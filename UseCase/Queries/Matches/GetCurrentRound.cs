

using System.Net;
using Domain.Shared.Enum;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;

namespace UseCase.Queries.Matches
{
    public class GetCurrentRound
    {
        public record CurrentRoundCommand(Guid tournamentId) : IRequest<int>;

        public class Handler(ITournamentRepository tournamentRepository) : IRequestHandler<CurrentRoundCommand, int>
        {
            public async Task<int> Handle(CurrentRoundCommand request, CancellationToken cancellationToken)
            {
                var tournament = await tournamentRepository.GetTournamentRounds(m => m.Id == request.tournamentId)
                    ?? throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound); 
                if(!tournament.IsTournamentStarted)
                {
                    return 0;
                }

                if (tournament.Rounds.Count == 0)
                {
                    return 0; 
                }

                var currentRound = tournament.Rounds
                .FirstOrDefault(r => r.Matches.Any(m => m.MatchStatus != MatchStatus.Completed));

                if (currentRound == null)
                {
                    return 0; 
                }
                return currentRound.RoundNumber; 
            }
        }
    }
}
