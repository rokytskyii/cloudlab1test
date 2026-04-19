using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using SmartphoneStore.Api.Tablet.Contract;
using SmartphoneStore.Dal.Tablet;

namespace SmartphoneStore.IntegrationTests.Tablet;

public class TabletsControllerTests : BaseTest
{
    private HttpClient _httpClient;

    public TabletsControllerTests()
    {
        _httpClient = InitTestServer().GetClient();
    }

    [Fact]
    public async Task CreateAsync_ValidData_ReturnsOk()
    {
        var request = new CreateTablet { Brand = "Apple", ModelName = "iPad", Price = 800, ReleaseDate = DateTime.UtcNow, HasStylus = true };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/tablets", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        CosmosDbContext.Tablets.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk()
    {
        var dao = new TabletDao { Id = Guid.NewGuid(), Brand = "Asus", ModelName = "Tab", Price = 300 };
        CosmosDbContext.Tablets.Add(dao);
        await CosmosDbContext.SaveChangesAsync();

        var response = await _httpClient.GetAsync($"/api/v1/tablets/{dao.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOk()
    {
        CosmosDbContext.Tablets.Add(new TabletDao { Id = Guid.NewGuid(), Brand = "Lenovo", ModelName = "M10", Price = 250 });
        await CosmosDbContext.SaveChangesAsync();

        var response = await _httpClient.GetAsync("/api/v1/tablets?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk()
    {
        var dao = new TabletDao { Id = Guid.NewGuid(), Brand = "Old", ModelName = "Old", Price = 100, ReleaseDate = DateTime.UtcNow };
        CosmosDbContext.Tablets.Add(dao);
        await CosmosDbContext.SaveChangesAsync();
        CosmosDbContext.ChangeTracker.Clear(); 

        var request = new UpdateTablet { Id = dao.Id, Brand = "New", ModelName = "New", Price = 200, ReleaseDate = DateTime.UtcNow };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/api/v1/tablets/{dao.Id}", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent()
    {
        var dao = new TabletDao { Id = Guid.NewGuid(), Brand = "Acer", ModelName = "Zen", Price = 150 };
        CosmosDbContext.Tablets.Add(dao);
        await CosmosDbContext.SaveChangesAsync();

        var response = await _httpClient.DeleteAsync($"/api/v1/tablets/{dao.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        CosmosDbContext.Tablets.Count().Should().Be(0);
    }
}