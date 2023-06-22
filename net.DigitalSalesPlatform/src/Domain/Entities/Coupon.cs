using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table("Coupons", Schema = "dbo")]
public class Coupon : BaseEntity
{
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsUsed { get; set; }
}