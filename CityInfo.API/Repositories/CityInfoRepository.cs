using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoDbContext _context;
    public CityInfoRepository(CityInfoDbContext context)
    {
        this._context = context ?? throw new ArgumentException(nameof(context));
    }


    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
    }



    public async Task<City?> GetCityAsync(int cityId, bool includepointsOfInterest)
    {
        if (includepointsOfInterest)
        {
            return await _context.Cities.Include(c => c.PointOfInterest)
                .Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        return await _context.Cities
               .Where(c => c.Id == cityId).FirstOrDefaultAsync();

    }

    public async Task<PointOfInterest?> GetPointOfInterestsForCity(int cityId, int pointsOfInterestId)
    {
        return await _context.PointsOfInterest.Where
            (c => c.CityId == cityId && c.Id == pointsOfInterestId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId)
    {
        return await _context.PointsOfInterest.Where
                   (c => c.CityId == cityId)
                   .ToListAsync();
    }

    public async Task<bool> IsCityExistAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);

    }
    public async Task AddPoinOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        if (city != null)
        {
            city.PointOfInterest.Add(pointOfInterest);
        }
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterest.Remove(pointOfInterest);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0);
    }
}
