using System.Net;
using Xunit;
using FluentAssertions;

namespace SmartphoneStore.IntegrationTests.SmartphoneStats;

public class SmartphoneStatsControllerTests : BaseTest
{
    private readonly HttpClient _httpClient;

    public SmartphoneStatsControllerTests()
    {
        _httpClient = InitTestServer().GetClient();
    }

    [Fact]
    public async Task GetStats_ShouldReturnOk_AndListOfStrings()
    {
        var response = await _httpClient.GetAsync("/api/v1/smartphones-stats");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadAsStringAsync();
        data.Should().NotBeNull();
        data.Should().BeEquivalentTo("[]");
    }
}