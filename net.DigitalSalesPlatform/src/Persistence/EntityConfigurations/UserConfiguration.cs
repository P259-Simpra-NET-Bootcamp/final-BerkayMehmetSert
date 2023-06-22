using Core.Utilities.Date;
using Core.Utilities.Security;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PasswordSalt).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Role).IsRequired().HasMaxLength(5);
        builder.Property(x => x.PointBalance).IsRequired().HasDefaultValue(0).HasPrecision(15, 2);
        builder.Property(x => x.LastTransactionDate).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

        builder.HasIndex(x => x.Email).IsUnique();

        HashingHelper.CreatePasswordHash("1234", out var passwordHash, out var passwordSalt);

        var admin = new User
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateHelper.GetCurrentDate(),
            CreatedBy = "System",
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@system.com",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "Admin"
        };

        builder.HasData(admin);
    }
}