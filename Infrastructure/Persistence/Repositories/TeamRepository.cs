

using Domain.Entities;
using System.Linq.Expressions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;
using Domain.Aggregate.TournamentAggregate;

namespace Infrastructure.Persistence.Repositories
{
    public class TeamRepository(ApplicationDbContext context) : ITeamRepository
    {
        

        public async Task<Team?> GetAsync(Expression<Func<Team, bool>> predicate)
        {
            return await context.Team
                .Include(t => t.Players)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> IsExistsAsync(Expression<Func<Team, bool>> expression)
        {
            return await context.Team.AnyAsync(expression);
        }

    }
}
