using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Tablet;
using SmartphoneStore.Model.MessageBroker;

namespace SmartphoneStore.Orchestrator.Tablet;

public class TabletOrchestrator : ITabletOrchestrator
{
    private readonly ITabletRepository _repository;
    private readonly IPublisher _publisher;

    public TabletOrchestrator(ITabletRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<TabletDto> CreateAsync(TabletDto tablet)
    {
        var result = await _repository.CreateAsync(tablet);

        await _publisher.PublishAsync($"Created Tablet with Id: {result.Id}");

        return result;
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

        var result = await _repository.UpdateAsync(tablet);

        await _publisher.PublishAsync($"Updated Tablet with Id: {result.Id}");

        return result;
    }

    public async Task<TabletDto> DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) throw new EntityNotFoundException($"Tablet with id {id} not found.");

        await _repository.DeleteAsync(id);
        return existing;
    }
}