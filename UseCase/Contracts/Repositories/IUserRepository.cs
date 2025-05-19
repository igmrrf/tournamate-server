using Domain.Entities;
using System.Linq.Expressions;

namespace UseCase.Contracts.Repositories;

public interface IUserRepository
{
    Task<User?> GetAsync(Expression<Func<User, bool>> predicate);
    Task<User> CreateAsync(User user);
    Task<bool> IsExistsAsync(Expression<Func<User, bool>> expression);
    Task<List<User>> GetManyAsync(Expression<Func<User, bool>> predicate);

    Task UpdateUserAsync(User user);
}