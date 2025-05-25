

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
        Task<Tournament?> GetTournamentWithParticipantAsync(Expression<Func<Tournament, bool>> predicate);
        Task<ICollection<Tournament>> ListOfTournament(Expression<Func<Tournament, bool>> predicate);
        Task<Tournament?> GetTournamentDetail(Expression<Func<Tournament, bool>> predicate);
        Task<Tournament?> GetTournamentRole(Expression<Func<Tournament, bool>> predicate);
    }
}
