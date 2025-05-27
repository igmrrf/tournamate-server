

using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;

namespace UseCase.Contracts.Repositories
{
    public interface ITeamRepository
    {
        Task<Team?> GetAsync(Expression<Func<Team, bool>> predicate);
        Task<bool> IsExistsAsync(Expression<Func<Team, bool>> expression);
    }
}
