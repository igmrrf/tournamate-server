using System.Reflection;
using Domain.Aggregate.TournamentAggregate;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    public DbSet<User> Users { get; set; }
    public DbSet<Tournament> Tournament { get; set; }
    public DbSet<Team> Team { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}