using System.Reflection;
using Domain.Aggregate.TournamentAggregate;
using Domain.Entities;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    public DbSet<User> Users { get; set; }
    public DbSet<Tournament> Tournament { get; set; }
    public DbSet<Team> Team { get; set; }
    public DbSet<Match> Match { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<TournamentRole> Role { get; set; }
    public DbSet<Player> Player { get; set; }
    public DbSet<TournamentRound> Round { get; set; }
    public DbSet<TeamPerformance> Performance { get; set; }
    public DbSet<MatchScore> Scores { get; set; }
    public DbSet<MatchFoul> Fouls { get; set; }
    public DbSet<MatchTimeStamp> MatchTime { get; set; }
    public DbSet<MatchSubstitution> Subtitues { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<TournamentInfo> TournamentInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "api"))
    .AddJsonFile("appsettings.Development.json", optional: false)
    .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("DEFAULT_CONNECTION env variable is not set.");
        optionsBuilder.UseMySQL(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}