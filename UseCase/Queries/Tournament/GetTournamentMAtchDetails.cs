

//using Domain.Enum;
//using System.Net;
//using MediatR;
//using UseCase.Contracts.Repositories;
//using UseCase.Exceptions;
//using Domain.Shared.Enum;

//namespace UseCase.Queries.Tournament
//{
//    public class GetTournamentMAtchDetails
//    {
//        public record MatchCreateCommand : IRequest<TournamentMatchResponse> 
//        {
//            public Guid TournamentId { get; set; }
//        }

//        public class Handler(ITournamentRepository tournamentRepository) : IRequestHandler<MatchCreateCommand, TournamentMatchResponse>
//        {
//            public async Task<TournamentMatchResponse> Handle(MatchCreateCommand request, CancellationToken cancellationToken)
//            {
//                var tournament = await tournamentRepository.GetAsync(t => t.Id == request.TournamentId)
//                    ?? throw new UseCaseException("Tournament Not Found", "NotFound", (int)HttpStatusCode.NotFound);

//                if (tournament.Status == TournamentStatus.Draft)
//                {
//                    throw new UseCaseException("Tournament Is Not Yet Published",
//                        "NotYetPublished", (int)HttpStatusCode.BadRequest);
//                }

//                // Generate matches if not already generated
//                if (!tournament.Rounds.Any())
//                {
//                    tournament.GenerateTournamentMatches();
//                    await tournamentRepository.UpdateAsync(tournament);
//                }

//                // Get the next scheduled match that needs attention
//                var nextMatch = tournament.Rounds
//                    .SelectMany(r => r.Matches)
//                    .FirstOrDefault(m => m.Status == MatchStatus.Scheduled ||
//                                       (m.Status == MatchStatus.Pending && m.HomeTeamId != null));

//                if (nextMatch == null)
//                {
//                    throw new UseCaseException("No matches available to process",
//                        "NoMatchesAvailable", (int)HttpStatusCode.NotFound);
//                }

//                return new TournamentMatchResponse
//                {
//                    MatchId = nextMatch.Id,
//                    HomeTeamId = nextMatch.HomeTeamId,
//                    AwayTeamId = nextMatch.AwayTeamId,
//                    Status = nextMatch.Status.ToString(),
//                    RoundNumber = tournament.Rounds.First(r => r.Matches.Contains(nextMatch)).RoundNumber,
//                    TournamentId = tournament.Id
//                };
//            }
//        }

//        public record TournamentMatchResponse
//        {

//        }
//    }
//}
