cd 
using System.Threading.Tasks;
using Domain.Shared.Entities;

namespace Domain.Aggregate.TournamentAggregate
{
    public class TournamentRound : BaseEntity
    {
        private readonly List<Match> _matches = new();
        private readonly List<Team> _byesTeamId = new();

        public int RoundNumber { get; set; }
        public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();
        public IReadOnlyCollection<Team> Byes => _byesTeamId.AsReadOnly();

        public TournamentRound(int roundNumber)
        {
            RoundNumber = roundNumber;
        }

        public TournamentRound() { }

        public void AddMatch(Match match)
        {
            _matches.Add(match);
        }

        public void AddBye(Guid teamId)
        {
            var team = _byesTeamId.FirstOrDefault(t => t.Id == teamId);
            if (team != null)
            {
                _byesTeamId.Add(team);
            }
        }
    }
}
