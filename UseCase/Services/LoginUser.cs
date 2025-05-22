using UseCase.Exceptions;
using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using Infrastructure.InternalServices.Jwt;
using Microsoft.AspNetCore.Http;
using UseCase.Contracts.Services;
using Domain.Entities;
namespace UseCase.Services
{
    public class LoginUser
    {
        public record LoginRequestModel(string email, string password) : IRequest<LoginResponseModel>;

        public record LoginResponseModel(string accessToken, string RefreshToken);

        public class Handler(IUserRepository userRepository, IJwtConfig jwtBearer, IUnitOfWork unitOfWork) : IRequestHandler<LoginRequestModel, LoginResponseModel>
        {
            public async Task<LoginResponseModel> Handle(LoginRequestModel request, CancellationToken cancellationToken)
            {
                var getUser = await userRepository.GetAsync(u => u.Email == request.email) ?? throw new NullReferenceException($" Email {request.email} Can NOT Be Found");

                if ( !getUser.IsVerified)
                {
                    throw new UseCaseException($"User Not Verified", 
                                "NotVerified", (int)HttpStatusCode.NotFound);
                }

                var hashPassword = BCrypt.Net.BCrypt.Verify(request.password, getUser.PasswordHash);

                if(hashPassword)
                {
                    var refreshToken = getUser.SetRefreshToken();
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    var token = await jwtBearer.GenerateJwtAsync(getUser);

                    return new LoginResponseModel(token, refreshToken);
                }

                throw new UseCaseException($"User Not FOund", 
                                "NotFound", (int)HttpStatusCode.NotFound);
            }
        }
    }
}