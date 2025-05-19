

namespace Domain.ValueObject
{
    public class MatchSubstitution
    {
        public Guid TeamId { get; private set; }
        public Guid PlayerInId { get; private set; }
        public Guid PlayerOutId { get; private set; }
        public DateTime Time { get; private set; }

        public MatchSubstitution() { }
        
        public MatchSubstitution(Guid teamId, Guid playerInId, Guid playerOutId, DateTime time)
        {
            TeamId = teamId;
            PlayerInId = playerInId;
            PlayerOutId = playerOutId;
            Time = time;
        }

    }
}