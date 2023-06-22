using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("ProductCategoryMap", Schema = "dbo")]
public class ProductCategoryMap
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}