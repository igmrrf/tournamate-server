
using Domain.Shared.Enum;

namespace UseCase.DTOs
{
    public class StartTournamentDTO
    {
        public record EnrichedMatchDto(
            Guid MatchId,
            TeamInfo? HomeTeam,
            TeamInfo? AwayTeam);

        public record TeamInfo(
            Guid TeamId,
            string Name,
            string LogoUrl,
            int playersCount,
            int SubplayersCount,
            string ManagerName,
            List<PlayerInfo> players);


        public record PlayerInfo(
             string Name ,
         string Position ,
         string JerseyNumber 
            );
    }
}
