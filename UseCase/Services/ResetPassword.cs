
using MediatR;
using System.Net;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;

namespace UseCase.Services
{
    public class ResetPassword
    {
        public record ResetPasswordCommand(string newPassword, string resetCode) : IRequest;

        public class Handler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<ResetPasswordCommand>
        {
            public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var getUser = await userRepository.GetAsync(u => u.PasswordResetToken == request.resetCode);
                if (getUser is null)
                {
                    throw new UseCaseException($"Code is Invalid",
                        "CodeInvalid", (int)HttpStatusCode.NotFound);
                }

                if (getUser.PasswordResetTokenExpiry < DateTime.UtcNow)
                {
                    throw new UseCaseException($"Verification Code TimeOut",
                        "VerificationCodeTimeOut", (int)HttpStatusCode.NotFound);
                }

                var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.newPassword, salt);

                getUser.SetNewPassword(hashPassword, salt, getUser.Id);
                await unitOfWork.SaveChangesAsync(cancellationToken);

            }
        }
    }
}
