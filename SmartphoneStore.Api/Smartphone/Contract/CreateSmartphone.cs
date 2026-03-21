using System.ComponentModel.DataAnnotations;

namespace SmartphoneStore.Api.Smartphone.Contract;

public class CreateSmartphone
{
    [Required]
    [MinLength(2), MaxLength(50)]
    public string Brand { get; set; }

    [Required]
    [MinLength(2), MaxLength(100)]
    public string ModelName { get; set; }

    [Range(1, 10000)]
    public decimal Price { get; set; }

    [Range(typeof(DateTime), "01/01/2000", "01/01/2100")]
    public DateTime ReleaseDate { get; set; }

    [Range(8, 2048)]
    public int StorageGB { get; set; }
}