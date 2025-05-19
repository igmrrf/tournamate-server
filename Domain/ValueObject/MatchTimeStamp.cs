
using Domain.Shared.Enum;
namespace Domain.ValueObject
{
    public class MatchTimeStamp
    {
        public DateTime StartTime { get; private set; }
        public DateTime StartDate { get; private set; }
        public bool HasMatchbegan { get; private set; }
        public bool HasMatchEnded { get; private set; }
        public WhoWon WhoWon { get; private set; }
        public Guid UserId { get; private set; }

        public MatchTimeStamp(){}

        public MatchTimeStamp(DateTime startTime, DateTime startDate, bool hasMatchbegan, bool hasMatchEnded, WhoWon whoWon, Guid userId)
        {
            StartTime = startTime;
            StartDate = startDate;
            HasMatchbegan = hasMatchbegan;
            HasMatchEnded = hasMatchEnded;
            WhoWon = whoWon;
            UserId = userId;
        }


    }
}