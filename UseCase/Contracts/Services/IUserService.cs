

using Domain.Entities;

namespace UseCase.Contracts.Services
{
    public interface IUserService
    {
        Task<User?> LoggedInUser();
    }
}