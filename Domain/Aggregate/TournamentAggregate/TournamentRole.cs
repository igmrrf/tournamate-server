using Domain.Entities;
using Domain.Shared.Entities;
using Domain.Shared.Enum;
using Domain.ValueObject;
namespace Domain.Aggregate.TournamentAggregate
{
    public class TournamentRole : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid UserId { get; set; }
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; } = new();
        public virtual Tournament Tournament { get; set; }

        public TournamentRole() { }
    }

}