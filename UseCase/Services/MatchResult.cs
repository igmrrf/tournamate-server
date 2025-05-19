

//using MediatR;
//using static UseCase.Queries.Tournament.GetTournamentMAtchDetails;
//using System.Net;
//using UseCase.Contracts.Repositories;
//using UseCase.Exceptions;

//namespace UseCase.Services
//{
//    public class MatchResult
//    {
//        public class RecordMatchResultCommandHandler : IRequestHandler<RecordMatchResultCommand, TournamentMatchResponse>
//        {
//            private readonly ITournamentRepository _tournamentRepository;

//            public RecordMatchResultCommandHandler(ITournamentRepository tournamentRepository)
//            {
//                _tournamentRepository = tournamentRepository;
//            }

//            public async Task<TournamentMatchResponse> Handle(RecordMatchResultCommand request, CancellationToken cancellationToken)
//            {
//                var tournament = await _tournamentRepository.GetAsync(t => t.Id == request.TournamentId)
//                    ?? throw new UseCaseException("Tournament Not Found", "NotFound", (int)HttpStatusCode.NotFound);

//                var match = tournament.Rounds
//                    .SelectMany(r => r.Matches)
//                    .FirstOrDefault(m => m.Id == request.MatchId)
//                    ?? throw new UseCaseException("Match Not Found", "MatchNotFound", (int)HttpStatusCode.NotFound);

//                match.RecordResult(request.WinnerId, request.HomeScore, request.AwayScore);
//                tournament.AdvanceWinners(match.Id, request.WinnerId);

//                await _tournamentRepository.UpdateAsync(tournament);

//                return new TournamentMatchResponse
//                {
//                    MatchId = match.Id,
//                    WinnerId = match.WinnerId,
//                    Status = match.Status.ToString(),
//                    TournamentId = tournament.Id
//                };
//            }
//        }
//    }
//}
