

using UseCase.DTOs;

namespace UseCase.Contracts.Services
{
    public interface IGenerateToken
    {
        Task<ConfirmationTokenResponse> GenerateEmailConfirmationToken(string email);
        Task<ConfirmationTokenResponse> GeneratePasswordResetToken(string email);
    }
}
