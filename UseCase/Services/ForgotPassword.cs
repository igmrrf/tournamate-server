
using System.Net;
using MediatR;
using UseCase.Contracts.InternalServices;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Services
{
    public class ForgotPassword
    {
        public record ForgotPasswordCommand(string Email, string Url) : IRequest;

        public class Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailProvider emailProvider, IGenerateToken generateToken) : IRequestHandler<ForgotPasswordCommand>
        {
            public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var getUser = await userRepository.GetAsync(u => u.Email == request.Email);
                if (getUser is null || getUser.IsVerified is false)
                {
                    throw new UseCaseException($"User Not Found",
                        "NotFound", (int)HttpStatusCode.NotFound);
                }

                var token = await generateToken.GeneratePasswordResetToken(request.Email);

                var callBackUrl = $"{request.Url}?userId={token.UserId}&token={token.Token}";

                await emailProvider.SendForgotPasswordLink(getUser.UserName,  request.Email, callBackUrl);
            }
        }
    }
}
