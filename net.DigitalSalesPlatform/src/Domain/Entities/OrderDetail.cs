using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table("OrderDetails", Schema = "dbo")]
public class OrderDetail : BaseEntity
{
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}