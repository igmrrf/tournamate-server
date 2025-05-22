

using System.Net;
using Domain.Entities;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;

namespace UseCase.Queries.Matches
{
    public class MatchesInTournamentRound
    {
        public record GetMatchesInRoundHandler(Guid TournamentId, int RoundNumber) : IRequest<List<MatchInRoundResponse>>;

        public class Handler(IMatchRepository matchRepository, ITournamentRepository tournamentRepository) : IRequestHandler<GetMatchesInRoundHandler, List<MatchInRoundResponse>>
        {
            public async Task<List<MatchInRoundResponse>> Handle(GetMatchesInRoundHandler request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetAsync(t => t.Id == request.TournamentId) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                if (!getTournament.IsTournamentStarted)
                {
                    throw new UseCaseException($"Tournament Not Started.",
                    "NotStarted", (int)HttpStatusCode.BadRequest);
                }

                var getMatches = await matchRepository.ListOfMatch(m => m.TournamentId == request.TournamentId);
                if (getMatches.Count == 0)
                {
                    throw new UseCaseException($"Match Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);
                }

                var round = getTournament.Rounds.FirstOrDefault(r => r.RoundNumber == request.RoundNumber)
                                    ?? throw new UseCaseException($"Match Not Found.",
                                        "NotFound", (int)HttpStatusCode.NotFound);

                
                return round.Matches.Select(m => new MatchInRoundResponse
                {
                    MatchId = m.Id,
                    RoundNumber = round.RoundNumber,
                    HomeTeamId = m.HomeId,
                    AwayTeamId = m.AwayId,
                    Status = m.MatchStatus.ToString(),
                    HomeTeamScore = m.MatchScore.HomeTeamScore,
                    AwayTeamScore = m.MatchScore.AwayTeamScore,
                    TournamentId = m.TournamentId,
                    StartTime = m.MatchTimeStamp.StartTime,
                    EndTime = m.MatchTimeStamp.EndTime,
                    HomeTeamName = getTournament.Teams.FirstOrDefault(t => t.Id == m.HomeId)?.Name,
                    AwayTeamName = getTournament.Teams.FirstOrDefault(t => t.Id == m.AwayId)?.Name
                }).ToList();
            }
        }

        public record MatchInRoundResponse
        {
            public Guid MatchId { get; set; }
            public Guid TournamentId { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string HomeTeamName { get; set; }
            public string AwayTeamName { get; set; }
            public string Status { get; set; }  
            public int HomeTeamScore { get; set; }
            public int AwayTeamScore { get; set; }
            public int RoundNumber { get; set; }
        }
    }
}
