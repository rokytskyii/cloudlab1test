namespace SmartphoneStore.Model.Smartphone;

public interface ISmartphoneRepository
{
    Task<SmartphoneDto> CreateAsync(SmartphoneDto smartphone);
    Task<SmartphoneDto> GetByIdAsync(int id);
    Task<IEnumerable<SmartphoneDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<SmartphoneDto> UpdateAsync(SmartphoneDto smartphone);
    Task DeleteAsync(int id);
}