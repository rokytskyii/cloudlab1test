using System.Net;
using Xunit;
using FluentAssertions;
using SmartphoneStore.IntegrationTests;

namespace SmartphoneStore.IntegrationTests.SmartphoneStats;

public class SmartphoneStatsControllerTests : BaseTest 
{
    [Fact]
    public async Task GetStats_ShouldReturnOk_AndListOfStrings()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/v1/smartphones-stats");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadAsStringAsync();
        data.Should().NotBeNull();
        data.Should().BeEquivalentTo("[]");
    }
}