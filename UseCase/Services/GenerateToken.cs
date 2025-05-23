

using System.Net;
using System.Security.Cryptography;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.DTOs;
using UseCase.Exceptions;

namespace UseCase.Services
{
    public class GenerateToken(IUserRepository userRepository, IUnitOfWork unitOfWork) : IGenerateToken
    {
        public async Task<ConfirmationTokenResponse> GenerateEmailConfirmationToken(string email)
        {
            var getEmail = await userRepository.GetAsync(u => u.Email == email);
            

            var token = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(token);
            var tokenString = Convert.ToBase64String(token);

            var expiryTime = DateTime.UtcNow.AddMinutes(30);

            getEmail.SetVerificationToken(tokenString, expiryTime, getEmail.Id);

            await unitOfWork.SaveChangesAsync();

            return new ConfirmationTokenResponse
            {
                Email = email,
                UserId = getEmail.Id,
                Token = tokenString,
                Name = getEmail.UserName,
            };
        }

        public async Task<ConfirmationTokenResponse> GeneratePasswordResetToken(string email)
        {
            var getEmail = await userRepository.GetAsync(u => u.Email == email);
            if (getEmail is null)
            {
                throw new UseCaseException($"{email} Not Found",
                        "NotFound", (int)HttpStatusCode.NotFound);
            }


            var token = new byte[5]; 
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(token);
            var code = BitConverter.ToInt32(token, 0).ToString("D6"); 


            var expiryTime = DateTime.UtcNow.AddMinutes(30);

            getEmail.SetPasswordResetToken(code, expiryTime, getEmail.Id);

            await unitOfWork.SaveChangesAsync();

            return new ConfirmationTokenResponse
            {
                Email = email,
                UserId = getEmail.Id,
                Token = code,
                Name = getEmail.UserName,
            };
        }
    }
}
