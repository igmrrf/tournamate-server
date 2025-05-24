

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Exceptions;

namespace UseCase.Services
{
    public class VerifyEmail
    {
        public record VerifyEmailCommand(Guid userId, string token) : IRequest;

        public class Handler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<VerifyEmailCommand>
        {
            public async Task Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
            {
                var getUser = await userRepository.GetAsync(u => u.Id == request.userId 
                && u.VerificationToken == request.token);
                if (getUser is null)
                {
                    throw new UseCaseException($"User Not Found",
                        "NotFound", (int)HttpStatusCode.NotFound);
                }

                if (getUser.VerificationTokenExpiry < DateTime.UtcNow) 
                {
                    throw new UseCaseException($"Verification Link TimeOut",
                        "VerificationLinkTimeOut", (int)HttpStatusCode.NotFound);
                }

                getUser.Verify(request.userId);
                await unitOfWork.SaveChangesAsync(cancellationToken);

            }
        }
    }
}
