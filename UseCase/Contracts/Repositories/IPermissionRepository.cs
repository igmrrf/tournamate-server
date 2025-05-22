

using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;

namespace UseCase.Contracts.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> GetAsync(Expression<Func<Permission, bool>> predicate);
    }
}
