

using Domain.Aggregate.TournamentAggregate;
using System.Linq.Expressions;

namespace UseCase.Contracts.Repositories
{
    public interface IMatchRepository
    {
        Task<Match?> GetAsync(Expression<Func<Match, bool>> predicate);
        Task<List<Match>> ListOfMatch(Expression<Func<Match, bool>> predicate);
        Task<bool> IsExistsAsync(Expression<Func<Match, bool>> expression);
    }
}
