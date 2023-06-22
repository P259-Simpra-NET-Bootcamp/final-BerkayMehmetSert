using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(9);
        builder.Property(x => x.TotalAmount).IsRequired();
        builder.Property(x => x.CouponAmount).IsRequired().HasDefaultValue(0).HasPrecision(15, 2);
        builder.Property(x => x.CouponCode).HasMaxLength(10);
        builder.Property(x => x.EarnedPoint).IsRequired().HasDefaultValue(0).HasPrecision(15, 2);
        builder.Property(x => x.SpentPoint).IsRequired().HasDefaultValue(0).HasPrecision(15, 2);
        builder.Property(x => x.OrderDate).IsRequired();
        builder.Property(x => x.Status)
            .HasConversion(
                y => y.ToString(),
                y => (OrderStatus)Enum.Parse(typeof(OrderStatus), y)).HasMaxLength(9);

        builder.HasIndex(x => x.OrderNumber).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}