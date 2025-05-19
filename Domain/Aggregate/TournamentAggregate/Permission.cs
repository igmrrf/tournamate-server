

namespace Domain.Aggregate.TournamentAggregate
{
    public class Permission
    {
        public string UserName { get; private set; }
        public Guid TournamentId { get; private set; }
        public bool CanUpdateScore { get; private set; }
        public bool CanRecordFoul { get; private set; }
        public bool CanMakeSubstitutions { get; private set; }
        public bool CanUpdateTimeStamp { get; private set; }

        public Permission(string userName, Guid tournamentId, bool canUpdateScore,
            bool canRecordFoul, bool canMakeSubstitutions, bool canUpdateTimeStamp)
        {
            UserName = userName;
            TournamentId = tournamentId;
            CanUpdateScore = canUpdateScore;
            CanRecordFoul = canRecordFoul;
            CanMakeSubstitutions = canMakeSubstitutions;
            CanUpdateTimeStamp = canUpdateTimeStamp;
        }

        public Permission(){}
    }
}