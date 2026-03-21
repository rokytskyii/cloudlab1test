using AutoMapper;
using SmartphoneStore.Api.Smartphone.Contract;
using SmartphoneStore.Model.Smartphone;

namespace SmartphoneStore.Api.Smartphone;

public class SmartphoneMap : Profile
{
    public SmartphoneMap()
    {
        CreateMap<CreateSmartphone, SmartphoneDto>();
        CreateMap<UpdateSmartphone, SmartphoneDto>();
        CreateMap<SmartphoneDto, GetSmartphone>();
    }
}