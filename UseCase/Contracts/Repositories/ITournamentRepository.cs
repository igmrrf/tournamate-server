

using Domain.Entities;
using System.Linq.Expressions;

namespace UseCase.Contracts.Repositories
{
    public interface ITournamentRepository
    {
        Task<Tournament?> GetAsync(Expression<Func<Tournament, bool>> predicate);
        Task<Tournament> CreateAsync(Tournament tournament);
        Task<bool> IsExistsAsync(Expression<Func<Tournament, bool>> expression);
        Task UpdateTournament(Tournament tournament);
        Task DeleteAsync(Tournament tournament);
        Task<ICollection<Tournament>> ListOfTournament(Expression<Func<Tournament, bool>> predicate);
        Task<Tournament?> GetTournamentDetail(Expression<Func<Tournament, bool>> predicate);
    }
}
