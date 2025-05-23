

using Domain.Shared.Entities;

namespace Domain.ValueObject
{
    public class MatchFoul : BaseEntity
    {
        public int YellowCards { get; private set; }
        public int RedCards { get; private set; }
        public Guid TeamId { get; private set; }
        public Guid PlayerId { get; private set; }
        public DateTime Time { get; private set; }

        public MatchFoul(){}
        public MatchFoul(int yellowCards, int redCards, Guid teamId, Guid playerId, DateTime time)
        {
            YellowCards = yellowCards;
            RedCards = redCards;
            TeamId = teamId;
            PlayerId = playerId;
            Time = time;
        }
    }
}