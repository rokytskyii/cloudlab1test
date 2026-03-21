namespace SmartphoneStore.Api.Smartphone.Contract;

public class GetSmartphone
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string ModelName { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int StorageGB { get; set; }
}