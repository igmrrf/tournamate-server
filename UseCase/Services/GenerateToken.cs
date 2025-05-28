

using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Services
{
    public class GenerateToken(IUserRepository userRepository, IUnitOfWork unitOfWork) : IGenerateToken
    {
        public async Task<string> GenerateEmailConfirmationToken(string email)
        {
            var getEmail = await userRepository.GetAsync(u => u.Email == email);

            if (getEmail == null) 
            {
                throw new UseCaseException($"{email} Not Found",
                        "NotFound", (int)HttpStatusCode.NotFound);
            }

            var code = GenerateRandom5DigitCode();
            var expiryTime = DateTime.UtcNow.AddMinutes(30);

            getEmail.SetVerificationToken(code, expiryTime, getEmail.Id);

            await unitOfWork.SaveChangesAsync();

            return code;
        }

        public string GenerateRandom5DigitCode()
        {
            Random random = new Random();
            int code = random.Next(10000, 100000); 
            return code.ToString();
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            var getEmail = await userRepository.GetAsync(u => u.Email == email);
            if (getEmail is null)
            {
                throw new UseCaseException($"{email} Not Found",
                        "NotFound", (int)HttpStatusCode.NotFound);
            }


            var code = GenerateRandom5DigitCode();
            var expiryTime = DateTime.UtcNow.AddMinutes(30);

            getEmail.SetPasswordResetToken(code, expiryTime, getEmail.Id);

            await unitOfWork.SaveChangesAsync();

            return code;
        }
    }
}
