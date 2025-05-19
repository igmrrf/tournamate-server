

using Domain.Shared.Entities;
using Domain.Shared.Enum;
using Domain.Shared.Exceptions;
using Domain.ValueObject;

namespace Domain.Aggregate.TournamentAggregate
{
    public class Match : BaseEntity
    {
        public Guid TournamentId { get; private set; }
        public int MatchNumber { get; private set; }
        public Guid HomeId { get; private set; }   
        public Guid AwayId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public MatchStatus MatchStatus { get; private set; }
        public MatchScore? MatchScore { get; private set; }
        public MatchSubstitution? MatchSubstitutions { get; private set; } 
        public MatchFoul MatchFouls { get; private set; } = new();
        public MatchTimeStamp? MatchTimeStamp { get; private set; }
        public TeamPerformance TeamPerformance { get; private set; }
        

        public Match() { }
        public Match(Guid tournamentId, Guid homeId, Guid awayId, DateTime startTime, DateTime endTime, MatchStatus matchStatus)
        {
            TournamentId = tournamentId;
            HomeId = homeId;
            AwayId = awayId;
            StartTime = startTime;
            EndTime = endTime;
            MatchStatus = matchStatus;
            MatchScore = new MatchScore(0, 0);
        }

        public void UpdateMatch(Guid homeId, Guid awayId, DateTime startTime, DateTime endTime, MatchStatus matchStatus)
        {
            HomeId = homeId;
            AwayId = awayId;
            StartTime = startTime;
            EndTime = endTime;
            MatchStatus = matchStatus;
        }

        public void UpdateMatchScore(MatchScore matchScore)
        {
            MatchScore = matchScore;
        }

        public void UpdateMatchSubstitution(MatchSubstitution matchSubstitution)
        {
            MatchSubstitutions = matchSubstitution;
        }

        public void UpdateMatchFoul(MatchFoul matchFoul)
        {
            MatchFouls = matchFoul;
        }

        public void UpdateMatchTimeStamp(MatchTimeStamp matchTimeStamp)
        {
            MatchTimeStamp = matchTimeStamp;
        }

        public void SetHomeTeam(Guid teamId)
        {
            if (MatchStatus != MatchStatus.Pending)
                throw new DomainException("Can only set teams in pending matches");

            HomeId = teamId;
            if (AwayId != null)
            {
                MatchStatus = MatchStatus.Shedulled;
            }
        }

        public void SetAwayTeam(Guid teamId)
        {
            if (MatchStatus != MatchStatus.Pending)
                throw new DomainException("Can only set teams in pending matches");

            AwayId = teamId;
            if (HomeId != null)
            {
                MatchStatus = MatchStatus.Shedulled;
            }
        }

        //public void RecordResult(Guid winnerId, int homeScore, int awayScore)
        //{
        //    if (MatchStatus != MatchStatus.Shedulled && MatchStatus != MatchStatus.Ongoing)
        //        throw new DomainException("Match is not in a state to record results");

        //    if (winnerId != HomeId && winnerId != AwayId)
        //        throw new DomainException("Winner must be one of the competing teams");

        //    WinnerId = winnerId;
        //    MatchStatus = MatchStatus.Completed;
        //}

        public void ShedulleMatch()
        {
            MatchStatus = MatchStatus.Shedulled;
        }

    }
}