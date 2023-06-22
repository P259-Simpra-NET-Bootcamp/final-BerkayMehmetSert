using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table("Categories", Schema = "dbo")]
public class Category : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<ProductCategoryMap>? ProductCategories { get; set; }
}