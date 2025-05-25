namespace UseCase.Contracts.InternalServices
{
    public interface IEmailProvider
    {
        Task SendEmailVerificationMessage(string name, string email, string callbackUrl);
        Task SendForgotPasswordLink( string name, string email, string callbackUrl);
        Task TournamentRefereeNotificationEmail(string name,
            string email,
            string tournamentName,
            DateTime startDate,
            string permissionsDescription,
            string callbackUrl);
    }
}
