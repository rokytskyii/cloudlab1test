using FluentAssertions;
using Moq;
using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Tablet;
using SmartphoneStore.Orchestrator.Tablet;

namespace SmartphoneStore.OrchestratorTests.Tablet;

public class TabletOrchestratorTests
{
    private readonly Mock<ITabletRepository> _repositoryMock;
    private readonly TabletOrchestrator _orchestrator;

    public TabletOrchestratorTests()
    {
        _repositoryMock = new Mock<ITabletRepository>();
        _orchestrator = new TabletOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTabletDoesNotExist_ThrowsEntityNotFoundException()
    {
        var fakeId = Guid.NewGuid();
        _repositoryMock.Setup(repo => repo.GetByIdAsync(fakeId)).ReturnsAsync((TabletDto)null);

        Func<Task> act = async () => await _orchestrator.GetByIdAsync(fakeId);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}