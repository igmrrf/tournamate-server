using Domain.Shared.Entities;
using Domain.Aggregate.TournamentAggregate;
using System.Security.Cryptography;
namespace Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; private set; }
    public string Country { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordHashSalt { get; private set; }
    public bool IsVerified { get; private set; }
    public string? VerificationToken { get; private set; }
    public DateTime? VerificationTokenExpiry { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiry { get; private set; }
    public List<TournamentRole> Role { get; private set; }
    public DateTime RefreshTokenExpiration {get; private set;}
    public string? RefreshToken { get; private set; }
    //public List<Tournament> CreatedTournaments { get; private set; } = new();
    public List<Invitation> Invitations { get; private set; } = new();

    public User() { }

    public User(string userName, string email, string passwordHash, string passwordHashSalt, string country)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        PasswordHashSalt = passwordHashSalt;
        IsVerified = false;
        Country = country;
    }

    public void SetVerificationToken(string token, DateTime expiry, Guid modifiedBy)
    {
        ModifiedById = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
        VerificationToken = token;
        VerificationTokenExpiry = expiry;
    }

    public void SetPasswordResetToken(string token, DateTime expiry, Guid modifiedBy)
    {
        ModifiedById = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
        PasswordResetToken = token;
        PasswordResetTokenExpiry = expiry;
    }

    public void Verify(Guid modifiedBy)
    {
        IsVerified = true;
        ModifiedById = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetNewPassword(string passwordHash, string passwordHashSalt, Guid modifiedBy)
    {
        PasswordHash = PasswordHash;
        PasswordHashSalt = PasswordHashSalt;
        ModifiedById = modifiedBy;
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
    }

     public string SetRefreshToken()
 {
     RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
     return RefreshToken = GenerateRefreshToken();
 }

 private string GenerateRefreshToken()
 {
     var randomNumber = new byte[32];
     using var rng = RandomNumberGenerator.Create();
     rng.GetBytes(randomNumber);
     return Convert.ToBase64String(randomNumber);
 }
}