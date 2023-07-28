using AutoMapper;

namespace CityInfo.API.AutoMapperProfile;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>(); //for patch
        CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>(); //for post
        CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>(); //for put
    }

}
