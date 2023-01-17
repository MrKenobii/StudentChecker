using System.Net;
using AutoMapper;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class CityController : Controller
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public CityController(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
    public IActionResult GetCities()
    {
        // var cities = _mapper.Map<List<CityDto>>(_cityRepository.GetCities());
        var cities = _cityRepository.GetCities();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(cities);
    }

    [HttpGet("{cityId}")]
    [ProducesResponseType(200, Type = typeof(City))]
    [ProducesResponseType(400)]
    public IActionResult GetCityById(int cityId)
    {
        if (!_cityRepository.CityExists(cityId))
            return NotFound();
        var city = _cityRepository.GetCity(cityId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(city);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(City))]
    public IActionResult CreateCity([FromBody] CityDto cityDto)
    {
        var city = _mapper.Map<City>(_cityRepository.CreateCity(cityDto));
        return Created("HttpStatusCode.Created",city);
    }

    [HttpPut("{cityId}")]
    [ProducesResponseType(200, Type = typeof(City))]
    public IActionResult UpdateCity(int cityId, [FromBody] CityDto cityDto)
    {
        var city = _mapper.Map<City>(_cityRepository.UpdateCity(cityId, cityDto));
        return Ok(city);
    }

    [HttpDelete("{cityId}")]
    [ProducesResponseType(200, Type = typeof(City))]
    public void DeleteCity(int cityId)
    {
        _cityRepository.DeleteCity(cityId);
    }
    
}