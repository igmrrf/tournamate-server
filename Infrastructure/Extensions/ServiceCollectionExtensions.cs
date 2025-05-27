using Infrastructure.InternalServices.EmailService;
using Infrastructure.InternalServices.Jwt;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCase.Contracts.InternalServices;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using UseCase.Services;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{

   
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITeamRepository, TeamRepository>()
            .AddScoped<ITournamentRepository, TournamentRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IMatchRepository, MatchRepository>()
            .AddScoped<IPermissionRepository, PermissionRepository>()
            .AddScoped<IPlayerRepository, PlayerRepository>()
            ;
    }

    public static void AddServices(this IServiceCollection services)
    {
            services
            .AddScoped<IJwtConfig, JwtConfig>()
            .AddScoped<IEmailProvider, EmailProvider>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IGenerateToken, GenerateToken>()
            .AddScoped<IAuthorizationHandler, TournamentRoleHandler>()
            ;
    }



}