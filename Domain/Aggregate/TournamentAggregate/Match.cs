

using Domain.Shared.Entities;
using Domain.Shared.Enum;
using Domain.Shared.Exceptions;
using Domain.ValueObject;

namespace Domain.Aggregate.TournamentAggregate
{
    public class Match : BaseEntity
    {
        public Guid TournamentId { get; private set; }
        public int Round { get; private set; }
        public Guid HomeId { get; private set; }   
        public Guid AwayId { get; private set; }
        public MatchStatus MatchStatus { get; private set; }
        public MatchScore? MatchScore { get; private set; }
        public MatchSubstitution? MatchSubstitutions { get; private set; } 
        public MatchFoul MatchFouls { get; private set; } = new();
        public MatchTimeStamp? MatchTimeStamp { get; private set; }
        public TeamPerformance TeamPerformance { get; private set; }
        

        public Match() { }
        public Match(Guid tournamentId, Guid homeId, Guid awayId,  MatchStatus matchStatus)
        {
            TournamentId = tournamentId;
            HomeId = homeId;
            AwayId = awayId;
            MatchStatus = matchStatus;
            MatchScore = new MatchScore(0, 0);
        }

        public void CancelMatch(Guid matchId)
        {
            Id = matchId;
            MatchStatus = MatchStatus.Cancelled;
            //hgh
        }

        public void UpdateMatchScore(Guid matchId, MatchScore matchScore)
        {
            Id = matchId;
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
            if (MatchStatus != MatchStatus.Cancelled) 
            { 
                if(matchTimeStamp.HasMatchbegan)
                {
                    MatchStatus = MatchStatus.Ongoing;
                }
                if (matchTimeStamp.HasMatchEnded)
                {
                    MatchStatus = MatchStatus.Completed;
                }
                MatchTimeStamp = matchTimeStamp;
            }
        }

        public void SetHomeTeam(Guid teamId)
        {
            //if (MatchStatus != MatchStatus.Pending)
            //    throw new DomainException("Can only set teams in pending matches");

            HomeId = teamId;
            if (AwayId != null)
            {
                MatchStatus = MatchStatus.Shedulled;
            }
        }

        public void SetAwayTeam(Guid teamId)
        {
            //if (MatchStatus != MatchStatus.Pending)
            //    throw new DomainException("Can only set teams in pending matches");

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