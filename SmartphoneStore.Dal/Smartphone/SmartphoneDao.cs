using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartphoneStore.Dal.Smartphone;

[Table("smartphones", Schema = "dbo")]
public class SmartphoneDao
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("brand")]
    public string Brand { get; set; }

    [Column("model_name")]
    public string ModelName { get; set; }

    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column("release_date")]
    public DateTime ReleaseDate { get; set; }

    [Column("storage_gb")]
    public int StorageGB { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}
