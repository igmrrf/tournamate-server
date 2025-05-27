
using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;
using Domain.Shared.Exceptions;
using Domain.ValueObject;
using UseCase.DTOs;
using Domain.Enum;
using static UseCase.DTOs.StartTournamentDTO;

namespace UseCase.Commands.TournamentCommand
{
    public class StartTournament
    {
        public record StartTournamentCommand(Guid tournamentId) : IRequest<BaseResponse<TournamentSchedule>>;

        public class Handler(IUserRepository userRepository, ITeamRepository teamRepository, ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork) : IRequestHandler<StartTournamentCommand, BaseResponse<TournamentSchedule>>
        {
            public async Task<BaseResponse<TournamentSchedule>> Handle(StartTournamentCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetTournamentDetail(t => t.Id == request.tournamentId) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                if(getTournament.TournamentInfo.StartDate <= DateTime.UtcNow && getTournament.Status == TournamentStatus.Upcoming)
                {
                    try
                    {
                        var schedule = getTournament.StartTournament();
                        await unitOfWork.SaveChangesAsync(cancellationToken);

                        var enrichedMatches = new List<EnrichedMatchDto>();
                        foreach (var round in schedule.Rounds)
                        {
                            foreach (var match in round.Matches)
                            {
                                enrichedMatches.Add(new EnrichedMatchDto(
                                    match.MatchId,
                                    match.HomeTeamId != null
                                        ? await GetTeamInfo(match.HomeTeamId.Value)
                                        : null,
                                    match.AwayTeamId != null
                                        ? await GetTeamInfo(match.AwayTeamId.Value)
                                        : null
                                ));
                            }
                        }

                        return new BaseResponse<TournamentSchedule>
                        {
                            Data = schedule,
                            Message = "Tournament started successfully.",
                            IsSuccessful = true
                        };
                    }
                    catch (DomainException ex)
                    {
                        return new BaseResponse<TournamentSchedule>
                        {
                            Data = null,
                            Message = "failed to start tournament.",
                            IsSuccessful = false
                        };
                    }
                }

                throw new UseCaseException($"Not yet Time To Start.",
                    "NotYetTime", (int)HttpStatusCode.BadRequest);
            }

            private async Task<TeamInfo> GetTeamInfo(Guid teamId)
            {
                var team = await teamRepository.GetAsync(t => t.Id == teamId);
                var managerName = await userRepository.GetAsync(u => u.Id == team.UserId);

                return new TeamInfo(
                    team.Id,
                    team.Name,
                    team.Logo,
                    team.NoOfPlayer,
                    team.NoOfSubPlayer,
                    managerName.UserName,
                    team.Players.Select(p => new PlayerInfo(
                        p.Name,
                        p.Position,
                        p.JerseyNumber)).ToList());
            }
        }

    }
}
