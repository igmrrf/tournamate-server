using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using UseCase.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    internal class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task<User> CreateAsync(User user)
        {
             await context.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Update(user);
        }
        public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> IsExistsAsync(Expression<Func<User, bool>> expression)
        {
            return await context.Users.AnyAsync(expression);
        }

        public async Task<List<User>> GetManyAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users
                .Where(predicate)
                .ToListAsync();
        }
    }
}
