using Infrastructure.InternalServices.EmailService;
using Infrastructure.InternalServices.Jwt;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCase.Contracts.InternalServices;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Queries.Tournament;
using UseCase.Services;
using static UseCase.Services.LoginUser;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{

   
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITeamRepository, TeamRepository>()
            .AddScoped<ITournamentRepository, TournamentRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            ;
    }

    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
            services
            .AddScoped<IJwtConfig, JwtConfig>()
            .AddScoped<IEmailProvider, EmailProvider>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IGenerateToken, GenerateToken>()
            ;
    }



}