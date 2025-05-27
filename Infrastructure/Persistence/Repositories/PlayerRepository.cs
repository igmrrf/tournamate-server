

using Domain.Aggregate.TournamentAggregate;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class PlayerRepository(ApplicationDbContext context) : IPlayerRepository
    {
        public async Task<Player?> GetAsync(Expression<Func<Player, bool>> predicate)
        {
            return await context.Player.FirstOrDefaultAsync(predicate);
        }
    }
}
