

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
        }

        public void UpdateTeam(string name, string? logo)
        {
            Name = name ?? Name;
            Logo = logo ?? Logo;
        }

        public void AddPlayerToTeam(Player player)
        {
            if (NoOfPlayer > NoOfPlayer)
                throw new DomainException("Cannot add more players to the team.");
            Players.Add(player);
        }

        public void AddSubPlayerToTeam(Player player)
        {
            if (NoOfSubPlayer > NoOfSubPlayer)
                throw new DomainException("Cannot add more players to the team.");
            Players.Add(player);
        }

        public void UpdatePlayerInTeam(Guid playerId, string name, string position, string jerseyNumber)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
                throw new DomainException("Player not found in the team.");
            player.UpdatePlayer(name, position, jerseyNumber);
        }

        public void RemovePlayerFromTeam(Guid playerId)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
                throw new DomainException("Player not found in the team.");
            Players.Remove(player);
        }

    }
}