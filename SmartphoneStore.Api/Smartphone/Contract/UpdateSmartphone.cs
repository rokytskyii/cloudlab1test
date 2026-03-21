using System.ComponentModel.DataAnnotations;

namespace SmartphoneStore.Api.Smartphone.Contract;

public class UpdateSmartphone : CreateSmartphone
{
    [Required]
    public int Id { get; set; }
}