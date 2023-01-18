using AutoMapper;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class CollegeController : Controller
{
    private readonly ICollegeRepository _collegeRepository;
    private readonly IMapper _mapper;

    public CollegeController(ICollegeRepository collegeRepository, IMapper mapper)
    {
        _collegeRepository = collegeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<College>))]
    public IActionResult GetColleges()
    {
        var colleges = _collegeRepository.GetColleges();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(colleges);
    }
    [HttpGet("{collegeId}")]
    [ProducesResponseType(200, Type = typeof(College))]
    [ProducesResponseType(400)]
    public IActionResult GetCollegeById(int collegeId)
    {
        if (!_collegeRepository.CollegeExists(collegeId))
            return NotFound();
        var college = _mapper.Map<CollegeDto>( _collegeRepository.GetCollege((collegeId)));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(college);
    }
    [HttpGet("{collegeId}/city")]
    [ProducesResponseType(200, Type=typeof(IEnumerable<CityDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCityByCollegeId(int collegeId)
    {
        Console.WriteLine(collegeId);
        if (!_collegeRepository.CollegeExists(collegeId))
            return NotFound();
        var city = _collegeRepository.GetCityByCollege(collegeId);
        return Ok(city);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(College))]
    public IActionResult CreateCollege([FromBody] CollegeDto collegeDto)
    {
        var college = _mapper.Map<College>(_collegeRepository.CreateCollege(collegeDto));
        return Created("HttpStatusCode.Created",college);
    }
    [HttpPut("{collegeId}")]
    [ProducesResponseType(200, Type = typeof(College))]
    public IActionResult UpdateCollege(int collegeId, [FromBody] CollegeDto collegeDto)
    {
        var college = _mapper.Map<College>(_collegeRepository.UpdateCollege(collegeId, collegeDto));
        return Ok(college);
    }
    [HttpDelete("{collegeId}")]
    [ProducesResponseType(200)]
    public void DeleteCollege(int collegeId)
    {
        _collegeRepository.DeleteCollege(collegeId);
    }
}