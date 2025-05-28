


namespace UseCase.Contracts.Services
{
    public interface IGenerateToken
    {
        Task<string> GenerateEmailConfirmationToken(string email);
        Task<string> GeneratePasswordResetToken(string email);
    }
}
