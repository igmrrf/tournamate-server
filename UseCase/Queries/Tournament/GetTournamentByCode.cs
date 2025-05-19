

using MediatR;
using static UseCase.Queries.Tournament.GetTournamentId;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;
using UseCase.DTOs;
using Domain.Enum;

namespace UseCase.Queries.Tournament
{
    public class GetTournamentByCode
    {
        public record GetTournamentByCodeCommand(string code) : IRequest<BaseResponse<TournamentResponse>>;

        public class Handler(ITournamentRepository tournamentRepository) : IRequestHandler<GetTournamentByCodeCommand, BaseResponse<TournamentResponse>>
        {
            public async Task<BaseResponse<TournamentResponse>> Handle(GetTournamentByCodeCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentDetail(t => t.InvitationCode == request.code) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                if (getTournament.Status != TournamentStatus.Draft)
                {
                    if (getTournament.TournamentMode == TournamentMode.player_VS_player)
                    {
                        return new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
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
                            }
                        };
                    }
                    else if (getTournament.TournamentMode == TournamentMode.team_VS_team)
                    {
                        return new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
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
                                NoOfSubPlayer = getTournament.NoOfSubPlayers,
                                NoOfTeam = getTournament.NoOfTeams,
                            }
                        };
                    }
                }
                return new BaseResponse<TournamentResponse>
                {
                    IsSuccessful = false,
                    Data = null,
                };
            }
        }
    }
}
