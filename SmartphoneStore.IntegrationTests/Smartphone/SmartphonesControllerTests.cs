using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using SmartphoneStore.Api.Smartphone.Contract;
using SmartphoneStore.Dal.Smartphone;

namespace SmartphoneStore.IntegrationTests.Smartphone;

public class SmartphonesControllerTests : BaseTest
{
    private HttpClient _httpClient;

    public SmartphonesControllerTests()
    {
        _httpClient = InitTestServer().GetClient();
    }

    [Fact]
    public async Task GetByIdAsync_IfSmartphoneExists_ReturnsOk()
    {
        var dao = new SmartphoneDao { Brand = "Apple", ModelName = "iPhone", Price = 999, ReleaseDate = DateTime.UtcNow, StorageGB = 128 };
        SqlDbContext.Smartphones.Add(dao);
        await SqlDbContext.SaveChangesAsync();

        var response = await _httpClient.GetAsync($"/api/v1/smartphones/{dao.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = JsonConvert.DeserializeObject<GetSmartphone>(await response.Content.ReadAsStringAsync());
        result.Brand.Should().Be("Apple");
    }

    [Fact]
    public async Task CreateAsync_ValidData_ReturnsOkAndCreatesDbRecord()
    {
        var request = new CreateSmartphone { Brand = "Samsung", ModelName = "Galaxy S24", Price = 1000, ReleaseDate = DateTime.UtcNow, StorageGB = 256 };
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/smartphones", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        SqlDbContext.Smartphones.Count().Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ValidData_UpdatesRecordAndReturnsOk()
    {
        var dao = new SmartphoneDao
        {
            Brand = "OldBrand",
            ModelName = "OldModel",
            Price = 100,
            ReleaseDate = new DateTime(2022, 1, 1),
            StorageGB = 64
        };
        SqlDbContext.Smartphones.Add(dao);
        await SqlDbContext.SaveChangesAsync();
        
        SqlDbContext.ChangeTracker.Clear();

        var updateRequest = new UpdateSmartphone
        {
            Id = dao.Id,
            Brand = "NewBrand",
            ModelName = "NewModel",
            Price = 500,
            ReleaseDate = new DateTime(2023, 1, 1), 
            StorageGB = 128 
        };
        var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/api/v1/smartphones/{dao.Id}", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedInDb = SqlDbContext.Smartphones.First();
        updatedInDb.Brand.Should().Be("NewBrand");
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_PerformsSoftDelete()
    {
        var dao = new SmartphoneDao { Brand = "Nokia", ModelName = "3310" };
        SqlDbContext.Smartphones.Add(dao);
        await SqlDbContext.SaveChangesAsync();

        var response = await _httpClient.DeleteAsync($"/api/v1/smartphones/{dao.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var deletedInDb = SqlDbContext.Smartphones.First();
        deletedInDb.IsDeleted.Should().BeTrue(); 
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPaginatedList()
    {
        SqlDbContext.Smartphones.Add(new SmartphoneDao { Brand = "Apple", ModelName = "iPhone 13", Price = 800, ReleaseDate = DateTime.UtcNow, StorageGB = 128 });
        SqlDbContext.Smartphones.Add(new SmartphoneDao { Brand = "Samsung", ModelName = "S22", Price = 700, ReleaseDate = DateTime.UtcNow, StorageGB = 128 });
        SqlDbContext.Smartphones.Add(new SmartphoneDao { Brand = "Nokia", ModelName = "3310", Price = 50, ReleaseDate = DateTime.UtcNow, StorageGB = 1, IsDeleted = true });
        await SqlDbContext.SaveChangesAsync();

        var response = await _httpClient.GetAsync("/api/v1/smartphones?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<GetSmartphone>>(content);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
}