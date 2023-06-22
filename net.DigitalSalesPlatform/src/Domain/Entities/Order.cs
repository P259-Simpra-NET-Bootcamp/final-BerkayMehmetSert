using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;
using Domain.Enums;

namespace Domain.Entities;

[Table("Orders", Schema = "dbo")]
public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CouponAmount { get; set; }
    public string? CouponCode { get; set; }
    public decimal EarnedPoint { get; set; }
    public decimal SpentPoint { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public virtual List<OrderDetail>? OrderDetails { get; set; }
}