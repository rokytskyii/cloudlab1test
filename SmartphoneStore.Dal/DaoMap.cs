using AutoMapper;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Dal.Smartphone;

namespace SmartphoneStore.Dal;

public class DaoMap : Profile
{
    public DaoMap()
    {
        CreateMap<SmartphoneDto, SmartphoneDao>().ReverseMap();
    }
}
