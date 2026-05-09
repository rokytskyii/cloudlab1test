using FluentAssertions;
using Moq;
using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.MessageBroker;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Orchestrator.Smartphone;

namespace SmartphoneStore.OrchestratorTests.Smartphone;

public class SmartphoneOrchestratorTests
{
    [Fact]
    public async Task GetByIdAsync_IfSmartphoneDoesntExist_ThrowException()
    {
        const int id = 1;
        var repository = new Mock<ISmartphoneRepository>();

        repository
            .Setup(x => x.GetByIdAsync(id))!
            .ReturnsAsync((SmartphoneDto)null);

        var orchestrator = new SmartphoneOrchestrator(repository.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await orchestrator.GetByIdAsync(id));
    }

    [Fact]
    public async Task GetByIdAsync_IfSmartphoneExists_ReturnsSmartphone()
    {
        const int id = 1;
        var smartphone = new SmartphoneDto
        {
            Id = id,
            Brand = "Samsung",
            ModelName = "Galaxy S23",
            Price = 800,
            StorageGB = 256,
            ReleaseDate = new DateTime(2023, 1, 1)
        };
        var expectedSmartphone = new SmartphoneDto
        {
            Id = id,
            Brand = "Samsung",
            ModelName = "Galaxy S23",
            Price = 800,
            StorageGB = 256,
            ReleaseDate = new DateTime(2023, 1, 1)
        };

        var repository = new Mock<ISmartphoneRepository>();

        repository
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(smartphone);

        var orchestrator = new SmartphoneOrchestrator(repository.Object);

        var result = await orchestrator.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSmartphone);
    }

    [Fact]
    public async Task CreateAsync_ShouldPublishMessage_WhenSmartphoneCreated()
    {
        var mockRepo = new Mock<ISmartphoneRepository>();
        var mockPublisher = new Mock<IPublisher>();

        var newSmartphone = new SmartphoneDto { Id = 1, Brand = "TestBrand" };
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<SmartphoneDto>())).ReturnsAsync(newSmartphone);

        var orchestrator = new SmartphoneOrchestrator(mockRepo.Object, mockPublisher.Object);

        var result = await orchestrator.CreateAsync(newSmartphone);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);

        mockPublisher.Verify(p => p.PublishAsync(It.IsAny<string>()), Times.Once);
    }
}