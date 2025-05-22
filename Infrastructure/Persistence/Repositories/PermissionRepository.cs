

using System.Linq.Expressions;
using Domain.Aggregate.TournamentAggregate;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class PermissionRepository(ApplicationDbContext context) : IPermissionRepository
    {
        public async Task<Permission?> GetAsync(Expression<Func<Permission, bool>> predicate)
        {
            return await context.Permission.FirstOrDefaultAsync(predicate);
        }
    }
}
