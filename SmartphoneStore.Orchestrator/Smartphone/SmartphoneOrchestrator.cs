using SmartphoneStore.Model.Exception;
using SmartphoneStore.Model.Smartphone;

namespace SmartphoneStore.Orchestrator.Smartphone;

public class SmartphoneOrchestrator : ISmartphoneOrchestrator
{
    private readonly ISmartphoneRepository _repository;

    public SmartphoneOrchestrator(ISmartphoneRepository repository)
    {
        _repository = repository;
    }

    public async Task<SmartphoneDto> CreateAsync(SmartphoneDto smartphone)
    {
        return await _repository.CreateAsync(smartphone);
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

        return await _repository.UpdateAsync(smartphone);
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