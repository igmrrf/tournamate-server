using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityTypeConfigurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.PasswordHashSalt)
            .IsRequired();
        
        builder.Property(x => x.IsVerified)
            .IsRequired();
        
        builder.Property(x => x.VerificationToken).IsRequired(false);
        
        builder.Property(x => x.VerificationTokenExpiry).IsRequired(false);
        
        builder.Property(x => x.PasswordResetToken).IsRequired(false);
        
        builder.Property(x => x.PasswordResetTokenExpiry).IsRequired(false);
    }
}