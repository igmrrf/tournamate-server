

using System.Net;
using Domain.Entities;
using MediatR;
using UseCase.Contracts.InternalServices;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.UserCommand
{
    public class UserSignUp
    {
        public record UserSignUpCommand : IRequest<Guid>
        {
            public required string UserName { get; set; } 
            public required string Email { get; set; }
            public required string Country { get; set; }
            public required string Password { get; set; }
        }

        public class Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailProvider emailProvider, IGenerateToken tokenGenerator) : IRequestHandler<UserSignUpCommand, Guid>
        {
            public async Task<Guid> Handle(UserSignUpCommand command, CancellationToken cancellationToken)
            {
                var getEmail = await userRepository.IsExistsAsync(e =>  e.Email == command.Email);
                if (getEmail)
                {
                    throw new UseCaseException($"Applicant with Gmail: {command.Email} already Exist", 
                        "EmailALreadyExist", (int)HttpStatusCode.NotFound);
                }

                var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(command.Password, salt);

                var newUser = new User(command.UserName, command.Email, hashPassword, salt, command.Country);

                await userRepository.CreateAsync(newUser);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                var code = await tokenGenerator.GenerateEmailConfirmationToken(command.Email);

                await emailProvider.SendEmailVerificationMessage(command.UserName, command.Email, code);

                return newUser.Id;
            }
        }
    }
}
