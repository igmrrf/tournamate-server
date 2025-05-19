

namespace Domain.ValueObject
{
    public class MatchScore
    {
        public int HomeTeamScore { get; private  set; }
        public int AwayTeamScore { get; private set; }
        public bool IsPernaltyShootout { get; private set; }
        public bool IsDraw => HomeTeamScore == AwayTeamScore;
        public Guid ScorerId { get; private set; }
        public DateTime ScorerTime { get; private set; }
        public Guid TeamId { get; private set; }

        public MatchScore() { }
        public MatchScore(int homeTeamScore, int awayTeamScore)
        {
            HomeTeamScore = homeTeamScore;
            AwayTeamScore = awayTeamScore;
        }

        public void UpdatePlayerStats(int goals, Guid scorerId, DateTime scorerTime, Guid teamId)
        {
            if (goals < 0)
                throw new ArgumentException("Goals cannot be negative.");
            ScorerId = scorerId;
            ScorerTime = scorerTime;
            TeamId = teamId;
        }
        

        public void SetHomeTeamScore(int score)
        {
            HomeTeamScore += score;
        }

        public void SetAwayTeamScore(int score)
        {
            AwayTeamScore += score;
        }

        
    }
    
}
