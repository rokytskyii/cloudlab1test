namespace SmartphoneStore.Api.Tablet.Contract;

public class GetTablet
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string ModelName { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool HasStylus { get; set; }
}