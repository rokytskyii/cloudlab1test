using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Model.Tablet;
using SmartphoneStore.Model.TabletSmartphone;
using SmartphoneStore.Platform.BlobStorage;

namespace SmartphoneStore.Orchestrator.TabletSmartphone;

public class TabletSmartphoneOrchestrator : ITabletSmartphoneOrchestrator
{
    private readonly ITabletOrchestrator _tabletOrchestrator;
    private readonly ISmartphoneOrchestrator _smartphoneOrchestrator;
    private readonly IBlobStorage _blobStorage;

    public TabletSmartphoneOrchestrator(
        ITabletOrchestrator tabletOrchestrator,
        ISmartphoneOrchestrator smartphoneOrchestrator,
        IBlobStorage blobStorage)
    {
        _tabletOrchestrator = tabletOrchestrator;
        _smartphoneOrchestrator = smartphoneOrchestrator;
        _blobStorage = blobStorage;
    }

    public async Task<TabletSmartphoneDto> CreateLinkAsync(Guid tabletId, int smartphoneId)
    {
        var tablet = await _tabletOrchestrator.GetByIdAsync(tabletId);
        var smartphone = await _smartphoneOrchestrator.GetByIdAsync(smartphoneId);

        if (tablet == null || smartphone == null)
        {
            throw new EntityNotFoundException("Tablet or Smartphone not found.");
        }

        var fileName = $"{tabletId}_{smartphoneId}";
        var exists = await _blobStorage.ExistsAsync(fileName);

        if (!exists)
        {
            await _blobStorage.UploadBlobAsync(fileName);
        }

        return new TabletSmartphoneDto
        {
            TabletId = tabletId,
            SmartphoneId = smartphoneId
        };
    }

    public async Task<SmartphoneDto> GetSmartphoneFromTabletAsync(Guid tabletId, int smartphoneId)
    {
        var smartphone = await _smartphoneOrchestrator.GetByIdAsync(smartphoneId);

        var fileName = $"{tabletId}_{smartphoneId}";
        var exists = await _blobStorage.ExistsAsync(fileName);

        if (!exists)
        {
            throw new EntityNotFoundException($"Smartphone with id {smartphoneId} is not linked to Tablet {tabletId}");
        }

        return smartphone;
    }

    public async Task<IEnumerable<int>> GetSmartphonesByTabletAsync(Guid tabletId)
    {
        await _tabletOrchestrator.GetByIdAsync(tabletId);

        return await _blobStorage.GetAllFilesNameAsync(tabletId);
    }

    public async Task DeleteLinkAsync(Guid tabletId, int smartphoneId)
    {
        var fileName = $"{tabletId}_{smartphoneId}";
        var exists = await _blobStorage.ExistsAsync(fileName);

        if (!exists)
        {
            throw new EntityNotFoundException($"Link between Tablet {tabletId} and Smartphone {smartphoneId} does not exist.");
        }

        await _blobStorage.DeleteBlobAsync(fileName);
    }
}