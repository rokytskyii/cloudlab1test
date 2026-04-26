namespace SmartphoneStore.Model.Tablet;

public interface ITabletRepository
{
    Task<TabletDto> CreateAsync(TabletDto tablet);
    Task<TabletDto> GetByIdAsync(Guid id);
    Task<IEnumerable<TabletDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<TabletDto> UpdateAsync(TabletDto tablet);
    Task DeleteAsync(Guid id);
}
