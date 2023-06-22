using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table("Products", Schema = "dbo")]
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public decimal ScorePercentage { get; set; }
    public decimal MaxScore { get; set; }
    public virtual List<OrderDetail>? OrderDetails { get; set; }
    public virtual ICollection<ProductCategoryMap>? ProductCategories { get; set; }
}