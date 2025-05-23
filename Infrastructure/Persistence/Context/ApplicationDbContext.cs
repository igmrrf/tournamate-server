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

        // Load config from appsettings.json or environment

        var relativePathToApi = Path.Combine("..", "api"); // Adjust as per your actual structure
        Console.WriteLine($"Relative path segment to API: {relativePathToApi}");


        var apiProjectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "api"));

        Console.WriteLine($"Calculated API project directory: {apiProjectDirectory}");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectDirectory) // Set the base path to where appsettings.json actually resides
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) // Good practice for dev settings
            .Build();


        var connectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

        optionsBuilder.UseMySQL(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}