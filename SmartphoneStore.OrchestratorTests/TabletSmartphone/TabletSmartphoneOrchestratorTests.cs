using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Model.Tablet;
using SmartphoneStore.Orchestrator.TabletSmartphone;
using SmartphoneStore.Platform.BlobStorage;
using Xunit;

namespace SmartphoneStore.OrchestratorTests.TabletSmartphone
{
    public class TabletSmartphoneOrchestratorTests
    {
        private readonly Mock<ITabletOrchestrator> _tabletOrchestratorMock = new();
        private readonly Mock<ISmartphoneOrchestrator> _smartphoneOrchestratorMock = new();
        private readonly Mock<IBlobStorage> _blobStorageMock = new();
        private readonly TabletSmartphoneOrchestrator _orchestrator;

        public TabletSmartphoneOrchestratorTests()
        {
            _orchestrator = new TabletSmartphoneOrchestrator(
                _tabletOrchestratorMock.Object,
                _smartphoneOrchestratorMock.Object,
                _blobStorageMock.Object);
        }

        [Fact]
        public async Task CreateLinkAsync_ShouldThrowEntityNotFoundException_WhenTabletDoesNotExist()
        {
            // Arrange
            var tabletId = Guid.NewGuid();
            var smartphoneId = 1;

            _tabletOrchestratorMock.Setup(x => x.GetByIdAsync(tabletId)).ReturnsAsync((TabletDto)null);
            _smartphoneOrchestratorMock.Setup(x => x.GetByIdAsync(smartphoneId)).ReturnsAsync(new SmartphoneDto());

            // Act
            Func<Task> act = async () => await _orchestrator.CreateLinkAsync(tabletId, smartphoneId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
            _blobStorageMock.Verify(x => x.UploadBlobAsync(It.IsAny<string>()), Times.Never);
        }
    }
}