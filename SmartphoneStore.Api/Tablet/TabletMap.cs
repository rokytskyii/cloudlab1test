using AutoMapper;
using SmartphoneStore.Api.Tablet.Contract;
using SmartphoneStore.Model.Tablet;

namespace SmartphoneStore.Api.Tablet;

public class TabletMap : Profile
{
    public TabletMap()
    {
        CreateMap<CreateTablet, TabletDto>();
        CreateMap<UpdateTablet, TabletDto>();
        CreateMap<TabletDto, GetTablet>();
    }
}