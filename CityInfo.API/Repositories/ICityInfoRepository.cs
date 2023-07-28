using CityInfo.API.Entities;

namespace CityInfo.API.Repositories;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityAsync(int cityId, bool includepointsOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestsForCity(int cityId, int pointsOfInterestId);

    Task<bool> IsCityExistAsync(int cityId);


    Task AddPoinOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
    void DeletePointOfInterest(PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
}
