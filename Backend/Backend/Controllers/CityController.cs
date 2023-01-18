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
    [ProducesResponseType(200, Type = typeof(IEnumerable<CityResponse>))]
    public IActionResult GetCities()
    {
        var cities = _cityRepository.GetCities();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(cities);
    }

    [HttpGet("{cityId}")]
    [ProducesResponseType(200, Type = typeof(CityResponse))]
    [ProducesResponseType(400)]
    public IActionResult GetCityById(int cityId)
    {
        if (!_cityRepository.CityExists(cityId))
            return NotFound();
        var city = _cityRepository.GetCityResponse(cityId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(city);
    }
    
    [HttpGet("{cityId}/colleges")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetColleges(int cityId)
    {
        if (!_cityRepository.CityExists(cityId))
            return NotFound();
        var city = _cityRepository.GetCollegesByCity(cityId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(city);
    }
    
    [HttpGet("{cityId}/students")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<StudentDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetStudents(int cityId)
    {
        if (!_cityRepository.CityExists(cityId))
            return NotFound();
        var city = _cityRepository.GetStudentsByCity(cityId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(city);
    }
    
    [HttpGet("{cityId}/companies")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CollegeDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCompanies(int cityId)
    {
        if (!_cityRepository.CityExists(cityId))
            return NotFound();
        var city = _cityRepository.GetCompaniesByCity(cityId);
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