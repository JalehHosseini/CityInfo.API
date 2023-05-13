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

    public Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId)
    {
        throw new NotImplementedException();
    }
}
