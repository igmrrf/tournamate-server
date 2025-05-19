
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    internal class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
