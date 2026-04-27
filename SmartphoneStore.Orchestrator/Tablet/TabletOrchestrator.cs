using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Tablet;

namespace SmartphoneStore.Orchestrator.Tablet;

public class TabletOrchestrator : ITabletOrchestrator
{
    private readonly ITabletRepository _repository;

    public TabletOrchestrator(ITabletRepository repository)
    {
        _repository = repository;
    }

    public async Task<TabletDto> CreateAsync(TabletDto tablet)
    {
        return await _repository.CreateAsync(tablet);
    }

    public async Task<TabletDto> GetByIdAsync(Guid id)
    {
        var tablet = await _repository.GetByIdAsync(id);

        if (tablet == null) throw new EntityNotFoundException($"Tablet with id {id} not found.");

        return tablet;
    }

    public async Task<IEnumerable<TabletDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllAsync(pageNumber, pageSize);
    }

    public async Task<TabletDto> UpdateAsync(TabletDto tablet)
    {
        var existing = await _repository.GetByIdAsync(tablet.Id);
        if (existing == null) throw new EntityNotFoundException($"Tablet with id {tablet.Id} not found.");

        return await _repository.UpdateAsync(tablet);
    }

    public async Task<TabletDto> DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) throw new EntityNotFoundException($"Tablet with id {id} not found.");

        await _repository.DeleteAsync(id);

        return existing;
    }
}