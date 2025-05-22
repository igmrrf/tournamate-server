

using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository(ApplicationDbContext context) : IRoleRepository
    {
        public async Task<TournamentRole?> GetAsync(Expression<Func<TournamentRole, bool>> predicate)
        {
            return await context.Role.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<TournamentRole>> GetListAsync(Expression<Func<TournamentRole, bool>> predicate)
        {
            return await context.Role
                .Where(predicate)
                .ToListAsync();
        }
    }
}
