using System.ComponentModel.DataAnnotations;

namespace SmartphoneStore.Api.Tablet.Contract;

public class UpdateTablet : CreateTablet
{
    [Required]
    public Guid Id { get; set; } 
}