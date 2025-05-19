using Domain.Shared.Enum;

namespace Domain.Aggregate.TournamentAggregate
{
    public class Invitation
    {
        public Guid TournamentId { get; private set; }
        public Guid SenderId { get; private set; }
        public InvitationStatus InvitationStatus { get; private set; }
        public Guid RecipientId { get; private set; }
        public Role Role { get; private set; }
        public string InvitationCode { get; private set; } = default!;

        public Invitation(Guid tournamentId, Guid senderId, 
        InvitationStatus invitationStatus, Guid recipientId, Role role, string invitationCode)
        {
            TournamentId = tournamentId;
            SenderId = senderId;
            InvitationStatus = invitationStatus;
            RecipientId = recipientId;
            Role = role;
            InvitationCode = invitationCode;
        }
        
    }
}