using Domain.Entities;
using Domain.Shared.Entities;

namespace Domain.Aggregate.TournamentAggregate
{
    public class Permission : BaseEntity
    {
        public Guid TournamentId { get; private set; }
        public Guid UserId { get; private set; }
        public bool CanUpdateScore { get; private set; }
        public bool CanRecordFoul { get; private set; }
        public bool CanMakeSubstitutions { get; private set; }
        public bool CanUpdateTimeStamp { get; private set; }
        public virtual Tournament Tournament { get; private set; }

        public Permission( Guid tournamentId, bool canUpdateScore,
            bool canRecordFoul, bool canMakeSubstitutions, bool canUpdateTimeStamp, Guid userId)
        {
            TournamentId = tournamentId;
            CanUpdateScore = canUpdateScore;
            CanRecordFoul = canRecordFoul;
            CanMakeSubstitutions = canMakeSubstitutions;
            CanUpdateTimeStamp = canUpdateTimeStamp;
            UserId = userId;
        }

        public Permission() { }
    }
}