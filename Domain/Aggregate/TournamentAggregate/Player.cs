

using Domain.Shared.Entities;
using Domain.Shared.Enum;
using Domain.Shared.Exceptions;

namespace Domain.Aggregate.TournamentAggregate
{
    public class Player : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid TOurnmaentId { get; private set; }
        public Guid TeamId { get; private set; }
        public string Name { get; private set; } = default!;
        public string Position { get; private set; } = default!;
        public string JerseyNumber { get; private set; } = default!;
        public int NoOfPlayer { get; private set; }
        public bool IsAssinged { get; private set; }

        public Player() { }

        public Player(string name, string position, string jerseyNumber, Guid userId, Guid tournamentId, Guid teamId)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Name cannot be null or empty.", nameof(name));
            if (string.IsNullOrEmpty(position))
                throw new DomainException("Position cannot be null or empty.", nameof(position));
            if (string.IsNullOrEmpty(jerseyNumber))
                throw new DomainException("Jersey number cannot be null or empty.", nameof(jerseyNumber));
            {
                Name = name;
                Position = position;
                JerseyNumber = jerseyNumber;
                UserId = userId;
                TOurnmaentId = tournamentId;
                IsAssinged = true;
                TeamId = teamId;
            }
        }

        public void UpdatePlayer(string name, string position, string jerseyNumber)
        {
            Name = name ?? Name;
            Position = position ?? Position;
            JerseyNumber = jerseyNumber ?? JerseyNumber;
        }

    }
}
    