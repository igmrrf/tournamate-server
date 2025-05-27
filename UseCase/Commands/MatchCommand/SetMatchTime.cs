

//using System.Net;
//using Domain.Shared.Enum;
//using MediatR;
//using UseCase.Contracts.Repositories;
//using UseCase.Exceptions;
//using UseCase.Contracts.Services;

//namespace UseCase.Commands.MatchCommand
//{
//    public class SetMatchTime
//    {
//        public record SetMatchTimeCommand : IRequest
//        {
//            public DateTime StartTime { get; set; }
//            public DateTime StartDate { get; set; }
//            public bool HasMatchbegan { get; set; }
//            public bool HasMatchEnded { get; set; }
//            public WhoWon WhoWon { get; set; }
//            public Guid MatchId { get; set; }
//        }

//        public class Handler(IMatchRepository matchRepository, ITournamentRepository tournamentRepository, IUserService userService) : IRequestHandler<SetMatchTimeCommand>
//        {
//            public async Task Handle(SetMatchTimeCommand request, CancellationToken cancellationToken)
//            {
//                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

//                var getTournament = await tournamentRepository.GetAsync(t => t.Id == request.TournamentId
//                                    && t.UserId == user.Id) ??
//                    throw new UseCaseException($"Tournament Not Found.",
//                    "NotFound", (int)HttpStatusCode.NotFound);
//            }
//        }
//    }
//}
