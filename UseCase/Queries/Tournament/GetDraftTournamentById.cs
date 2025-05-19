

using Domain.Enum;
using MediatR;
using static UseCase.Queries.Tournament.GetTournamentId;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.DTOs;
using UseCase.Exceptions;
using static UseCase.Services.LoginUser;
using UseCase.Contracts.Services;

namespace UseCase.Queries.Tournament
{
    public class GetDraftTournamentById
    {
        public record GetDraftTournamentCommand(Guid TournamentId) : IRequest<BaseResponse<TournamentResponse>>;

        public class Handler(ITournamentRepository tournamentRepository, IUserService userService) : IRequestHandler<GetTournamentCommand, BaseResponse<TournamentResponse>>
        {
            public async Task<BaseResponse<TournamentResponse>> Handle(GetTournamentCommand request, CancellationToken cancellationToken)
            {
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetTournamentDetail(t => t.Id == request.TournamentId && t.UserId == user.Id) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);


                if (getTournament.Status == TournamentStatus.Draft)
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

                    if (getTournament.TournamentMode == TournamentMode.team_VS_team)
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
    }
}
