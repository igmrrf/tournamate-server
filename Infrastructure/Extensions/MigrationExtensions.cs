using Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(IServiceProvider serviceProvider, bool ensureDbCreated = false)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (ensureDbCreated)
            {
                EnsureDbCreation(context);
            }
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error occurred while migrating");
            throw;
        }
    }


    private static void EnsureDbCreation(ApplicationDbContext context)
    {
        if (context.Database.CanConnect())
        {
            return;
        }

        var databaseName = context.Database.GetDbConnection().Database;

        var connectionString = context.Database.GetConnectionString()!
            .Replace($"Initial Catalog={databaseName};", "", StringComparison.InvariantCultureIgnoreCase);

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"CREATE DATABASE [{databaseName}]";

        cmd.ExecuteNonQuery();

        connection.Close();
    }
}