using Domain.Aggregate.TournamentAggregate;
using Domain.Entities;
using Domain.ValueObject;
using UseCase.DTOs;

namespace UseCase.Contracts.Services
{
    public interface IUserService
    {
        Task<User?> LoggedInUser();
        Task<UserRoleAndPermission> GetUserRoleAndPermission(Guid userId, Guid tournamentId);
    }
}