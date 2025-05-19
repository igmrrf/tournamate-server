
using System.Threading.Tasks;
using Domain.Shared.Entities;

namespace Domain.Aggregate.TournamentAggregate
{
    public class TournamentRound : BaseEntity
    {
        private readonly List<Match> _matches = new();
        private readonly List<Guid> _byesTeamId = new();

        public int RoundNumber { get; }
        public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();
        public IReadOnlyCollection<Guid> Byes => _byesTeamId.AsReadOnly();

        public TournamentRound(int roundNumber)
        {
            RoundNumber = roundNumber;
        }

        public void AddMatch(Match match)
        {
            _matches.Add(match);
        }

        public void AddBye(Guid teamId)
        {
            _byesTeamId.Add(teamId);
        }
    }
}
