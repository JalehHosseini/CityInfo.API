using CityInfo.API.Models;
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
    private readonly CityDataStore _cityDataStore;

    public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService localMailService
        , CityDataStore cityDataStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
        this._cityDataStore = cityDataStore;
    }

    #region GetAllPoints
    [HttpGet]

    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        try
        {
            //throw new Exception(" exception sample...");

            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} not found");
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($" exeption Getting {cityId}", ex);
            return StatusCode(500, "Aproblem Happen While...");
        }

    }
    #endregion




    #region GetOnePoint

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    { //find city
        var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        //find poin of interest
        var point = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (point == null)
        {
            return NotFound();
        }

        return Ok(point);

    }
    #endregion


    #region Post

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        var maxPoinOfInterestId = _cityDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(c => c.Id);
        var createPoint = new PointOfInterestDto()
        {
            Id = ++maxPoinOfInterestId,

            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description,
        };
        city.PointsOfInterest.Add(createPoint);
        return CreatedAtAction("GetPointOfInterest", new
        {
            cityId = cityId,
            pointOfInterestId = createPoint.Id
        }, createPoint);
    }
    #endregion

    #region Put

    [HttpPut("{pointOfInterestId}")]

    public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId, int pointOfInterestId
        , PointOfInterestForUpdateDto pointOfInterest)
    {


        var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null) { return NotFound(); }

        var updatePoint = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);

        if (updatePoint == null) { return NotFound(); }

        updatePoint.Name = pointOfInterest.Name;
        updatePoint.Description = pointOfInterest.Description;

        return NoContent();
    }
    #endregion


    #region Patch
    [HttpPatch("{pointOfInterestId}")]

    public ActionResult<PointOfInterestDto> PartialUpdatePointOfInterest(int cityId, int pointOfInterestId
        , JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        //find city
        var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) { return NotFound(); }
        //find poin of interest from store (main value)
        var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null) { return NotFound(); }

        //find poin of interest for patch (replace value/input)
        var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
        {
            Name = pointOfInterestFromStore.Name,
            Description = pointOfInterestFromStore.Description,
        };
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
        pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(modelState: ModelState);
        }
        return NoContent();
    }


    #endregion


    #region Delete
    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        //find city
        var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        //find poin of interest
        var point = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (point == null)
        {
            return NotFound();
        }
        city.PointsOfInterest.Remove(point);
        _localMailService.Send("Point Of Interest Deleted", $"Point Of Interest{point.Name} with  id {point.Id} ");

        return NoContent();
    }
    #endregion

}
