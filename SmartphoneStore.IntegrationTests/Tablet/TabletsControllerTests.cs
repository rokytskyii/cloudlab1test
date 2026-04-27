using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using SmartphoneStore.Api.Tablet.Contract;
using SmartphoneStore.Dal.Tablet;
using Microsoft.EntityFrameworkCore;

namespace SmartphoneStore.IntegrationTests.Tablet;

public class TabletsControllerTests : BaseTest
{
    private HttpClient _httpClient;

    public TabletsControllerTests()
    {
        _httpClient = InitTestServer().GetClient();
    }

    [Fact]
    public async Task CreateAsync_ValidData_ReturnsOkAndMatchesData()
    {
        var request = new CreateTablet { Brand = "Apple", ModelName = "iPad", Price = 800, ReleaseDate = DateTime.UtcNow, HasStylus = true };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/tablets", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetTablet>(jsonResponse);
        result.Should().NotBeNull();
        result.Brand.Should().Be("Apple");
        CosmosDbContext.Tablets.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkAndCorrectItem()
    {
        var dao = new TabletDao { Id = Guid.NewGuid(), Brand = "Asus", ModelName = "Tab", Price = 300, ReleaseDate = DateTime.UtcNow };
        CosmosDbContext.Tablets.Add(dao);
        await CosmosDbContext.SaveChangesAsync();

        var response = await _httpClient.GetAsync($"/api/v1/tablets/{dao.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetTablet>(jsonResponse);
        result.Should().NotBeNull();
        result.Brand.Should().Be("Asus");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkAndList()
    {
        CosmosDbContext.Tablets.Add(new TabletDao { Id = Guid.NewGuid(), Brand = "Lenovo", ModelName = "M10", Price = 250, ReleaseDate = DateTime.UtcNow });
        await CosmosDbContext.SaveChangesAsync();

        var response = await _httpClient.GetAsync("/api/v1/tablets?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<GetTablet>>(jsonResponse);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOkAndUpdatesItem()
    {
        var dao = new TabletDao { Id = Guid.NewGuid(), Brand = "Old", ModelName = "Old", Price = 100, ReleaseDate = DateTime.UtcNow };
        CosmosDbContext.Tablets.Add(dao);
        await CosmosDbContext.SaveChangesAsync();
        CosmosDbContext.ChangeTracker.Clear();

        var request = new UpdateTablet { Brand = "NewBrand", ModelName = "NewModel", Price = 200, ReleaseDate = DateTime.UtcNow, HasStylus = true };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/api/v1/tablets/{dao.Id}", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetTablet>(jsonResponse);
        result.Should().NotBeNull();
        result.Brand.Should().Be("NewBrand");

        var updatedInDb = await CosmosDbContext.Tablets.FirstOrDefaultAsync(x => x.Id == dao.Id);
        updatedInDb.Brand.Should().Be("NewBrand");
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ReturnsOkAndDeletedModel()
    {
        var dao = new TabletDao { Id = Guid.NewGuid(), Brand = "Acer", ModelName = "Zen", Price = 150, ReleaseDate = DateTime.UtcNow };
        CosmosDbContext.Tablets.Add(dao);
        await CosmosDbContext.SaveChangesAsync();

        var response = await _httpClient.DeleteAsync($"/api/v1/tablets/{dao.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetTablet>(jsonResponse);
        result.Should().NotBeNull();
        result.Id.Should().Be(dao.Id);
        result.Brand.Should().Be("Acer");

        CosmosDbContext.Tablets.Count().Should().Be(0);
    }
}