

using Domain.Aggregate.TournamentAggregate;
using Domain.Entities;
using Domain.Shared.Enum;

namespace Domain.ValueObject
{
    
    public record TournamentSchedule(
    Guid TournamentId,
    List<RoundSchedule> Rounds);

    public record RoundSchedule(
        int RoundNumber,
        List<MatchSchedule> Matches);

    public record MatchSchedule(
        Guid MatchId,
        Guid? HomeTeamId,
        Guid? AwayTeamId,
        MatchStatus status);
}
