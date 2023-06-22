using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table("Users", Schema = "dbo")]
public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Role { get; set; }
    public decimal PointBalance { get; set; }
    public DateTime? LastTransactionDate { get; set; }
    public bool IsActive { get; set; }
    public List<Order>? Orders { get; set; }
}