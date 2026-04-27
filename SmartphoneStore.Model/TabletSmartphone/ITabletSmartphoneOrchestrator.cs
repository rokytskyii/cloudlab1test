using SmartphoneStore.Model.Smartphone;

namespace SmartphoneStore.Model.TabletSmartphone;

public interface ITabletSmartphoneOrchestrator
{
    Task<TabletSmartphoneDto> CreateLinkAsync(Guid tabletId, int smartphoneId);
    Task<SmartphoneDto> GetSmartphoneFromTabletAsync(Guid tabletId, int smartphoneId);
    Task<IEnumerable<int>> GetSmartphonesByTabletAsync(Guid tabletId);
    Task DeleteLinkAsync(Guid tabletId, int smartphoneId);
}
