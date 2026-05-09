using FluentAssertions;
using Moq;
using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.MessageBroker;
using SmartphoneStore.Model.Tablet;
using SmartphoneStore.Orchestrator.Tablet;

namespace SmartphoneStore.OrchestratorTests.Tablet;

public class TabletOrchestratorTests
{
    private readonly Mock<ITabletRepository> _repositoryMock;
    private readonly Mock<IPublisher> _publisherMock;
    private readonly TabletOrchestrator _orchestrator;

    public TabletOrchestratorTests()
    {
        _repositoryMock = new Mock<ITabletRepository>();
        _publisherMock = new Mock<IPublisher>();
        _orchestrator = new TabletOrchestrator(_repositoryMock.Object, _publisherMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTabletDoesNotExist_ThrowsEntityNotFoundException()
    {
        var fakeId = Guid.NewGuid();
        _repositoryMock.Setup(repo => repo.GetByIdAsync(fakeId)).ReturnsAsync((TabletDto)null);

        Func<Task> act = async () => await _orchestrator.GetByIdAsync(fakeId);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldPublishMessage_WhenTabletCreated()
    {
        var mockRepo = new Mock<ITabletRepository>();
        var mockPublisher = new Mock<IPublisher>();
        var tablet = new TabletDto { Id = Guid.NewGuid(), Brand = "Test" };

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<TabletDto>())).ReturnsAsync(tablet);
        var orchestrator = new TabletOrchestrator(mockRepo.Object, mockPublisher.Object);

        await orchestrator.CreateAsync(tablet);

        mockPublisher.Verify(p => p.PublishAsync(It.Is<string>(s => s.Contains("Created Tablet"))), Times.Once);
    }
}