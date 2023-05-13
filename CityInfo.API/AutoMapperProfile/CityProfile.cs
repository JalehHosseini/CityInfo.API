using AutoMapper;

namespace CityInfo.API.AutoMapperProfile;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<Entities.City, Models.CityWithoutPointOfInterest>();
        CreateMap<Entities.City, Models.CityDto>();
    }
}
