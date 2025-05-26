

using Domain.Enum;
using MediatR;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.DTOs;
using UseCase.Exceptions;
using static UseCase.Queries.Tournament.GetTournamentId;

namespace UseCase.Queries.Tournament
{
    public class UserTournament 
    {
        public record GetUserTournament(int status) : IRequest<ICollection<BaseResponse<TournamentResponse>>>;

        public class Handler(ITournamentRepository tournamentRepository, IUserService userService) : IRequestHandler<GetUserTournament, ICollection<BaseResponse<TournamentResponse>>>
        {
            public async Task<ICollection<BaseResponse<TournamentResponse>>> Handle(GetUserTournament request, CancellationToken cancellationToken)
            {
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.ListOfTournament(t => t.UserId == user.Id) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                if (!getTournament.Any())
                {
                    throw new UseCaseException($"You have not created any tournament.",
                    "NotFound", (int)HttpStatusCode.NotFound);
                }

                switch (request.status)
                {
                    case 0:
                        return getTournament.Select(t => new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
                            {
                                About = t.TournamentInfo.Information,
                                Access = t.TournamentInfo.IsPrivate ? "Private" : "Public",
                                EndDate = t.TournamentInfo.EndDate,
                                NoOfPlayer = t.NoOfPlayers,
                                SportName = t.TournamentInfo.SportName,
                                StartDate = t.TournamentInfo.StartDate,
                                thumbnail = t.TournamentInfo.TounamentThumbnail,
                                TournamentName = t.TournamentInfo.Name,
                                Status = (int)t.Status,
                                NoOfTeam = t.NoOfTeams,
                                NoOfSubPlayer = t.NoOfSubPlayers
                            }
                        }).ToList();

                    case 1:
                        return getTournament.Where(t => t.Status == TournamentStatus.Draft).Select(t => new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
                            {
                                About = t.TournamentInfo.Information,
                                Access = t.TournamentInfo.IsPrivate ? "Private" : "Public",
                                EndDate = t.TournamentInfo.EndDate,
                                NoOfPlayer = t.NoOfPlayers,
                                SportName = t.TournamentInfo.SportName,
                                StartDate = t.TournamentInfo.StartDate,
                                thumbnail = t.TournamentInfo.TounamentThumbnail,
                                TournamentName = t.TournamentInfo.Name,
                                Status = (int)t.Status,
                                NoOfTeam = t.NoOfTeams,
                                NoOfSubPlayer = t.NoOfSubPlayers
                            }
                        }).ToList();

                    case 2:
                        return getTournament.Where(t => t.Status == TournamentStatus.Completed).Select(t => new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
                            {
                                About = t.TournamentInfo.Information,
                                Access = t.TournamentInfo.IsPrivate ? "Private" : "Public",
                                EndDate = t.TournamentInfo.EndDate,
                                NoOfPlayer = t.NoOfPlayers,
                                SportName = t.TournamentInfo.SportName,
                                StartDate = t.TournamentInfo.StartDate,
                                thumbnail = t.TournamentInfo.TounamentThumbnail,
                                TournamentName = t.TournamentInfo.Name,
                                Status = (int)t.Status,
                                NoOfTeam = t.NoOfTeams,
                                NoOfSubPlayer = t.NoOfSubPlayers
                            }
                        }).ToList();

                    case 3:
                        return getTournament.Where(t => t.Status == TournamentStatus.Ongoing).Select(t => new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
                            {
                                About = t.TournamentInfo.Information,
                                Access = t.TournamentInfo.IsPrivate ? "Private" : "Public",
                                EndDate = t.TournamentInfo.EndDate,
                                NoOfPlayer = t.NoOfPlayers,
                                SportName = t.TournamentInfo.SportName,
                                StartDate = t.TournamentInfo.StartDate,
                                thumbnail = t.TournamentInfo.TounamentThumbnail,
                                TournamentName = t.TournamentInfo.Name,
                                Status = (int)t.Status,
                                NoOfTeam = t.NoOfTeams,
                                NoOfSubPlayer = t.NoOfSubPlayers
                            }
                        }).ToList();

                    case 4:
                        return getTournament.Where(t => t.Status == TournamentStatus.Upcoming).Select(t => new BaseResponse<TournamentResponse>
                        {
                            IsSuccessful = true,
                            Data = new TournamentResponse
                            {
                                About = t.TournamentInfo.Information,
                                Access = t.TournamentInfo.IsPrivate ? "Private" : "Public",
                                EndDate = t.TournamentInfo.EndDate,
                                NoOfPlayer = t.NoOfPlayers,
                                SportName = t.TournamentInfo.SportName,
                                StartDate = t.TournamentInfo.StartDate,
                                thumbnail = t.TournamentInfo.TounamentThumbnail,
                                TournamentName = t.TournamentInfo.Name,
                                Status = (int)t.Status,
                                NoOfTeam = t.NoOfTeams,
                                NoOfSubPlayer = t.NoOfSubPlayers
                            }
                        }).ToList();

                    default:
                        throw new UseCaseException($"Invalid status code.",
                            "InvalidStatusCode", (int)HttpStatusCode.BadRequest);
                }
            }
        }


    }
}
