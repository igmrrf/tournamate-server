
using Domain.Shared.Enum;
namespace Domain.ValueObject
{
    public class MatchTimeStamp
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool HasMatchbegan { get; private set; }
        public bool HasMatchEnded { get; private set; }
        public WhoWon WhoWon { get; private set; }
        public Guid UserId { get; private set; }
        public Guid MatchId { get; private set; }

        public MatchTimeStamp(){}

        public MatchTimeStamp(Guid matchId, DateTime startTime, DateTime endTime, bool hasMatchbegan, bool hasMatchEnded, WhoWon whoWon, Guid userId)
        {
            StartTime = startTime;
            EndTime = endTime;
            HasMatchbegan = hasMatchbegan;
            HasMatchEnded = hasMatchEnded;
            WhoWon = whoWon;
            UserId = userId;
            MatchId = matchId;
        }


    }
}