
using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;
using UseCase.DTOs;
using Domain.Enum;

namespace UseCase.Queries.Tournament
{
    public class GetTournamentId
    {
        public record GetTournamentCommand(Guid TournamentId) : IRequest<BaseResponse<TournamentResponse>>;

        public class Handler(ITournamentRepository tournamentRepository) : IRequestHandler<GetTournamentCommand, BaseResponse<TournamentResponse>>
        {
            public async Task<BaseResponse<TournamentResponse>> Handle(GetTournamentCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentDetail(t => t.Id == request.TournamentId) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);


                if (getTournament.Status != TournamentStatus.Draft)
                {
                    var response = new TournamentResponse
                    {
                        About = getTournament.TournamentInfo.Information,
                        Access = getTournament.TournamentInfo.IsPrivate ? "Private" : "Public",
                        EndDate = getTournament.TournamentInfo.EndDate,
                        NoOfPlayer = getTournament.NoOfPlayers,
                        SportName = getTournament.TournamentInfo.SportName,
                        StartDate = getTournament.TournamentInfo.StartDate,
                        thumbnail = getTournament.TournamentInfo.TounamentThumbnail,
                        TournamentName = getTournament.TournamentInfo.Name,
                        Status = (int)getTournament.Status,
                        NoOfTeam = getTournament.NoOfTeams
                    };

                    if (getTournament.TournamentMode == TournamentMode.TeamVsTeam)
                    {
                        response.NoOfSubPlayer = getTournament.NoOfSubPlayers;
                    }
                   
                    return new BaseResponse<TournamentResponse>
                    {
                        IsSuccessful = false,
                        Data = response,
                    };
                }

                throw new UseCaseException($"Tournament Not Found.",
                     "DraftTournament", (int)HttpStatusCode.NotFound);

            }
        }

        public record TournamentResponse
        {
            public string TournamentName { get; set; }
            public string SportName { get; set; }
            public string thumbnail { get; set; }
            public string About { get; set; }
            public int NoOfTeam { get; set; }
            public int NoOfPlayer { get; set; }
            public int? NoOfSubPlayer { get; set; }
            public DateTime StartDate { get;  set; }
            public DateTime EndDate { get;  set; }
            public string Access { get; set; }
            public int Status { get; set; }

        }
    }
}
