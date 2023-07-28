using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointOfInterestController : ControllerBase
{
    private readonly ILogger<PointOfInterestController> _logger;
    private readonly IMailService _localMailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;



    private readonly CityDataStore _cityDataStore;

    public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService localMailService
        , CityDataStore cityDataStore, ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));

        _cityInfoRepository = cityInfoRepository ??
            throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        this._cityDataStore = cityDataStore;
    }





    #region GetOnePoint

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        //find city

        if (!await _cityInfoRepository.IsCityExistAsync(cityId))
        {
            _logger.LogInformation($"{cityId} Not Found...!");
            return NotFound();
        }
        var pointOfInterest = _cityInfoRepository.GetPointOfInterestsForCity(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NoContent();
        }

        return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterest));

    }
    #endregion



    #region GetAllPoints
    [HttpGet]

    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        if (!await _cityInfoRepository.IsCityExistAsync(cityId))
        {
            _logger.LogInformation($"{cityId} Not Found...!");
            return NotFound();
        }
        var pointOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestsForCityAsync(cityId);
        return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestForCity));
    }
    #endregion





    #region Post

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
        PointOfInterestForCreationDto pointOfInterest)
    {
        if (!await _cityInfoRepository.IsCityExistAsync(cityId))
        {
            return NotFound();
        }

        var finalPoint = _mapper.Map<Entities.PointOfInterest>(pointOfInterest); //this is of type Entity
        await _cityInfoRepository.AddPoinOfInterestForCityAsync(cityId, finalPoint);
        await _cityInfoRepository.SaveChangesAsync();

        var createdPoint = _mapper.Map<Models.PointOfInterestDto>(finalPoint);//this is of type DTO
        return CreatedAtRoute("GetPointOfInterest", new
        {
            cityId = cityId,
            pointOfInterestId = createdPoint.Id
        }, createdPoint);
    }
    #endregion



    #region Put

    [HttpPut("{pointOfInterestId}")]

    public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId
        , PointOfInterestForUpdateDto pointOfInterest)
    {
        if (!await _cityInfoRepository.IsCityExistAsync(cityId))
        {
            return NotFound();
        }

        var point = _cityInfoRepository.GetPointOfInterestsForCity(cityId, pointOfInterestId);
        if (point == null)
        {
            return NotFound();
        }

        //_mapper.Map(pointOfInterest, point);
        _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }
    #endregion




    #region Patch
    [HttpPatch("{pointOfInterestId}")]

    public async Task<ActionResult> PartialUpdatePointOfInterest(int cityId, int pointOfInterestId
        , JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        //find city
        if (!await _cityInfoRepository.IsCityExistAsync(cityId))
        {
            return NotFound();
        }
        //find pointOfInterest
        var pointEntity = _cityInfoRepository.GetPointOfInterestsForCity(cityId, pointOfInterestId);
        if (pointEntity == null)
        {
            return NotFound();
        }

        var pointToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointEntity);
        patchDocument.ApplyTo(pointToPatch, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!TryValidateModel(pointToPatch))
        {
            return BadRequest(ModelState);

        }
        //_mapper.Map<pointToPatch>(pointEntity);
        return NoContent();
    }


    #endregion


    #region Delete
    [HttpDelete("{pontiOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(
           int cityId,
           int pontiOfInterestId)
    {

        if (!await _cityInfoRepository.IsCityExistAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity =
            await _cityInfoRepository
            .GetPointOfInterestsForCity(cityId, pontiOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();

        _localMailService
            .Send(
            "Point Of intrest deleted",
            $"Point Of Interest {pointOfInterestEntity.Name}with id {pointOfInterestEntity.Id}"
            );

        return NoContent();
    }
    #endregion

}
