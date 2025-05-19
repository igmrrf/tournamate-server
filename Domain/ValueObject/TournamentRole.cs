
using Domain.Shared.Enum;
using Domain.Aggregate.TournamentAggregate;
namespace Domain.ValueObject
{
    public class TournamentRole
    {
        public Guid TournamentId { get; set; }
        public Guid UserId { get; set; }
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; } = new();
    }
}