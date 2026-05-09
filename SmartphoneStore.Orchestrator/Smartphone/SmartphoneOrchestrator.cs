using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Model.MessageBroker;

namespace SmartphoneStore.Orchestrator.Smartphone;

public class SmartphoneOrchestrator : ISmartphoneOrchestrator
{
    private readonly ISmartphoneRepository _repository;
    private readonly IPublisher _publisher;

    public SmartphoneOrchestrator(ISmartphoneRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<SmartphoneDto> CreateAsync(SmartphoneDto smartphone)
    {
        var result = await _repository.CreateAsync(smartphone);

        await _publisher.PublishAsync($"Created Smartphone with Id: {result.Id}");

        return result;
    }

    public async Task<SmartphoneDto> GetByIdAsync(int id)
    {
        var smartphone = await _repository.GetByIdAsync(id);

        if (smartphone == null)
        {
            throw new EntityNotFoundException($"Smartphone with id {id} not found.");
        }

        return smartphone;
    }

    public async Task<IEnumerable<SmartphoneDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllAsync(pageNumber, pageSize);
    }

    public async Task<SmartphoneDto> UpdateAsync(SmartphoneDto smartphone)
    {
        var existing = await _repository.GetByIdAsync(smartphone.Id);
        if (existing == null)
        {
            throw new EntityNotFoundException($"Smartphone with id {smartphone.Id} not found.");
        }

        var result = await _repository.UpdateAsync(smartphone);

        await _publisher.PublishAsync($"Updated Smartphone with Id: {result.Id}");

        return result;
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new EntityNotFoundException($"Smartphone with id {id} not found.");
        }

        await _repository.DeleteAsync(id);
    }
}