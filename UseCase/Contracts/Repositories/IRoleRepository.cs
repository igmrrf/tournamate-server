

using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;

namespace UseCase.Contracts.Repositories
{
    public interface IRoleRepository
    {
        Task<TournamentRole?> GetAsync(Expression<Func<TournamentRole, bool>> predicate);
        Task<List<TournamentRole>> GetListAsync(Expression<Func<TournamentRole, bool>> predicate);
    }
}
