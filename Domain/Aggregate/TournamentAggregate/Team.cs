

using Domain.Shared.Entities;
using Domain.Shared.Exceptions;

namespace Domain.Aggregate.TournamentAggregate
{
    public class Team : BaseEntity
    {
        public  string Name { get; private set; } = default!;
        public string? Logo { get; private set; }
        public List<Player> Players { get; private set; } = new();
        //public List<Player> SubPlayers { get; private set; } = new();
        public int NoOfPlayer { get; private set; }
        public int NoOfSubPlayer { get; private set; }
        public int CurrentPlayerCount { get; private set; }
        public int CurrentSubPlayerCount { get; private set; }
        public Guid UserId { get; private set; }
        public Guid TournamentId { get; private set; }
        public string Code { get; private set; }
        public Team(){}
        public Team(string name, string? logo, Guid userId, Guid tournamentId, int noOfPlayers, int? noOfSubPlayers, string code)
        {
            Name = name;
            Logo = logo;
            UserId = userId;
            TournamentId = tournamentId;
            NoOfSubPlayer = NoOfSubPlayer;
            NoOfPlayer =  noOfPlayers;
            Code = code;
            CurrentSubPlayerCount = 0;
            CurrentPlayerCount = 0;
        }

        public void UpdateTeam(string name, string? logo)
        {
            Name = name ?? Name;
            Logo = logo ?? Logo;
        }

        public void AddPlayerToTeam(Player player)
        {
            if (CurrentPlayerCount >= NoOfPlayer)
            {
                throw new DomainException("Team player limit reached");
            }
            Players.Add(player);
            CurrentPlayerCount++;
        }

        public void AddSubPlayerToTeam(Player player)
        {
            if(CurrentSubPlayerCount >= NoOfSubPlayer)
            {
                throw new DomainException("Team substitute player limit reached");
            }
            Players.Add(player);
            CurrentSubPlayerCount++;
        }

        public void RemoveSubPlayerFromTeam(Guid playerId)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
                throw new DomainException("Substitute player not found in the team.");
            Players.Remove(player);
            CurrentSubPlayerCount--;
        }

        public void RemovePlayerFromTeam(Guid playerId)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
                throw new DomainException("Player not found in the team.");
            Players.Remove(player);
            CurrentPlayerCount--;
        }

    }
}