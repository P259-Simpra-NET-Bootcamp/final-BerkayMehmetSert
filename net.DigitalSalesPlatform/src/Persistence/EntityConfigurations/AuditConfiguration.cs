using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable("Audits", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.EntityName).IsRequired();
        builder.Property(x => x.ActionType).IsRequired().HasMaxLength(9);
        builder.Property(x => x.UserId).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Details).IsRequired().HasMaxLength(9);
        builder.Property(x => x.OriginalValue).IsRequired(false);
        builder.Property(x => x.OldValue).IsRequired(false);
        builder.Property(x => x.NewValue).IsRequired(false);
        builder.Property(x => x.ClientIP).IsRequired().HasMaxLength(255);
        builder.Property(x => x.UserAgent).IsRequired().HasMaxLength(256);
    }
}