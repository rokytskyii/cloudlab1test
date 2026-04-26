using AutoMapper;
using SmartphoneStore.Dal.Smartphone;
using SmartphoneStore.Dal.Tablet;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Model.Tablet;

namespace SmartphoneStore.Dal;

public class DaoMap : Profile
{
    public DaoMap()
    {
        CreateMap<SmartphoneDto, SmartphoneDao>().ReverseMap();
        CreateMap<TabletDao, TabletDto>().ReverseMap();
    }
}
