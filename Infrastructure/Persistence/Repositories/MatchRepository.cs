

using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class MatchRepository(ApplicationDbContext context) : IMatchRepository
    {
        public async  Task<Match> CreateAsync(Match match)
        {
            await context.AddAsync(match);
            return match;
        }

        public async Task DeleteAsync(Match match)
        {
             context.Remove(match);
        }

        public async Task<Match?> GetAsync(Expression<Func<Match, bool>> predicate)
        {
            return 
                 await context.Match
                .Include(x => x.MatchFouls)
                .Include(x => x.MatchTimeStamp)
                .Include(x => x.MatchSubstitutions)
                .Include(x => x.MatchScore)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> IsExistsAsync(Expression<Func<Match, bool>> expression)
        {
            return await context.Match.AnyAsync(expression);
        }

        public async Task<List<Match>> ListOfMatch(Expression<Func<Match, bool>> predicate)
        {
            var march = context.Match.Include(i => i.MatchScore)
                .Include(i => i.MatchTimeStamp)
                .Where(predicate);
            return await march.ToListAsync();
        }

        public async Task UpdateMatch(Match match)
        {
            context.Update(match);
        }
    }
}
