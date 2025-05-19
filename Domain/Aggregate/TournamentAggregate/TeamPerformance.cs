

namespace Domain.Aggregate.TournamentAggregate
{
    public class TeamPerformance
    {
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        //number of goals scored - number of goals not saved by the keeper
        public int Diff { get; set; }
        //3 point for win 1 point for draw
        public int TotalPoint { get; set; }
        public Guid TeamId { get; set; }
    }
}
