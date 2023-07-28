
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Authorize]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    //private readonly CityDataStore _cityDataStore;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        //this._cityDataStore = cityDataStore;

        _cityInfoRepository = cityInfoRepository ??
            throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
    {
        var cities = await _cityInfoRepository.GetCitiesAsync();

        return Ok(
            _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cities)
            );

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includepointsOfInterest = false)
    {
        var cities = await _cityInfoRepository.GetCityAsync(id, includepointsOfInterest);

        if (cities == null)
        {
            return NotFound();
        }
        if (includepointsOfInterest)
        {
            var res = _mapper.Map<CityDto>(cities);

            return Ok(res);
        }



        return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(cities));
    }
}
