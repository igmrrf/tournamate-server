

using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using static UseCase.Services.LoginUser;

namespace UseCase.Services
{
    public class ChangePassword
    {
        public record ChangePasswordCommand(string oldPassword, string newPassword) : IRequest;

        public class Handler(IUnitOfWork unitOfWork, IUserService userService, IUserRepository userRepository) : IRequestHandler<ChangePasswordCommand>
        {
            public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await userService.LoggedInUser() ??
                    throw new UseCaseException($"User Not Found.",
                    "UserNotFound", (int)HttpStatusCode.NotFound); 

                if (BCrypt.Net.BCrypt.Verify(request.oldPassword, user.PasswordHash))
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.newPassword);
                    user.SetNewPassword(passwordHash, user.PasswordHashSalt, user.Id);
                    await userRepository.UpdateUserAsync  (user);   
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new UseCaseException($"Old Password is incorrect.",
                    "OldPasswordIncorrect", (int)HttpStatusCode.NotFound);
                }
            }
        }
    }
}
