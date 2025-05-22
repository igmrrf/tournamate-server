

using System.Linq.Expressions;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class TournamentRepository(ApplicationDbContext context) : ITournamentRepository
    {

        public async Task<ICollection<Tournament>> ListOfTournament(Expression<Func<Tournament, bool>> predicate)
        {
            var tournament = context.Tournament.Include(i => i.TournamentInfo).Where(predicate);
            return await tournament.ToListAsync();
        }

        public async Task<Tournament?> GetTournamentDetail(Expression<Func<Tournament, bool>> predicate)
        {
            return await context.Tournament.Include(i => i.TournamentInfo).FirstOrDefaultAsync(predicate);
        }

        public async Task<Tournament?> GetTournamentRole(Expression<Func<Tournament, bool>> predicate)
        {
            return await context.Tournament.Include(p => p.Participants).FirstOrDefaultAsync(predicate);
        }

        public async Task<Tournament> CreateAsync(Tournament tournament)
        {
            await context.AddAsync(tournament);
            return tournament;
        }

        public async Task DeleteAsync(Tournament tournament)
        {
            context.Remove(tournament);
        }

        public async Task<Tournament?> GetAsync(Expression<Func<Tournament, bool>> predicate)
        {
            return await context.Tournament.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> IsExistsAsync(Expression<Func<Tournament, bool>> expression)
        {
            return await context.Tournament.AnyAsync(expression);
        }

        public async Task UpdateTournament(Tournament tournament)
        {
            context.Update( tournament);
        }
    }
}
