using System.Net;
using System.Text;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using SmartphoneStore.Api.Smartphone.Contract;

namespace SmartphoneStore.IntegrationTests.SmartphoneStats;

public class DeviceStatsControllerTests : BaseTest
{
    private readonly HttpClient _httpClient;

    public DeviceStatsControllerTests()
    {
        _httpClient = InitTestServer().GetClient();
    }

    [Fact]
    public async Task GetStats_ShouldReturnOk_AndListOfStrings()
    {
        TestStartup.MockedMessages.Clear();

        var response = await _httpClient.GetAsync("/api/v1/device-stats");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadAsStringAsync();
        data.Should().NotBeNull();
        data.Should().BeEquivalentTo("[]");
    }

    [Fact]
    public async Task CreateSmartphone_ShouldUpdateStats()
    {
        TestStartup.MockedMessages.Clear();
        var request = new CreateSmartphone
        {
            Brand = "Apple",
            ModelName = "iPhone 15",
            Price = 1000,
            ReleaseDate = DateTime.UtcNow,
            StorageGB = 256
        };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var postResponse = await _httpClient.PostAsync("/api/v1/smartphones", content);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var statsResponse = await _httpClient.GetAsync("/api/v1/device-stats");

        statsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await statsResponse.Content.ReadAsStringAsync();
        var stats = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

        stats.Should().NotBeNull();
        stats.Should().NotBeEmpty();
        stats.Should().Contain(msg => msg.Contains("Created Smartphone"));
    }
}