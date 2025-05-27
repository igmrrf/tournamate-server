
using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;

namespace UseCase.Contracts.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player?> GetAsync(Expression<Func<Player, bool>> predicate);
    }
}
