using AutoMapper;
using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class RecruiterController : Controller
{
    private readonly IRecruiterRepository _recruiterRepository;
    private readonly IMapper _mapper;

    public RecruiterController(IRecruiterRepository recruiterRepository, IMapper mapper)
    {
        _recruiterRepository = recruiterRepository;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Recruiter>))]
    public IActionResult GetRecruiters()
    {
        var recruiters = _recruiterRepository.GetRecruiters();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(recruiters);
    }
    [HttpGet("{recruiterId}")]
    [ProducesResponseType(200, Type = typeof(Recruiter))]
    [ProducesResponseType(400)]
    public IActionResult GetRecruiterById(int recruiterId)
    {
        if (!_recruiterRepository.RecruiterExists(recruiterId))
            return NotFound();
        var recruiter = _mapper.Map<RecruiterDto>( _recruiterRepository.GetRecruiter((recruiterId)));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(recruiter);
    }
    [HttpGet("{recruiterId}/companies")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCountriesByRecruiterId(int recruiterId)
    {
        if (!_recruiterRepository.RecruiterExists(recruiterId))
            return NotFound();
        var companies =  _recruiterRepository.GetCompanies(recruiterId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(companies);
    }    
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(RecruiterPostResponse))]
    public IActionResult CreateRecruiter([FromBody] RecruiterPostRequest recruiterDto)
    {
        var recruiter = _recruiterRepository.CreateRecruiter(recruiterDto);
        return Created("HttpStatusCode.Created",recruiter);
    }
    [HttpPut("{recruiterId}")]
    [ProducesResponseType(200, Type = typeof(RecruiterPostRequest))]
    public IActionResult UpdateRecruiter(int recruiterId, [FromBody] RecruiterPostRequest recruiterDto)
    {
        var recruiter = _recruiterRepository.UpdateRecruiter(recruiterId, recruiterDto);
        return Ok(recruiter);
    }
    [HttpDelete("{recruiterId}")]
    [ProducesResponseType(200)]
    public void DeleteRecruiter(int recruiterId)
    {
        _recruiterRepository.DeleteRecruiter(recruiterId);
    }
    [HttpPut("{recruiterId}/update-profile")]
    [ProducesResponseType(200, Type = typeof(Recruiter))]
    public IActionResult UpdateProfile(int recruiterId, [FromBody] RecruiterUpdateProfile recruiterUpdateProfile)
    {
        var recruiter = _recruiterRepository.UpdateRecruiterProfile(recruiterId, recruiterUpdateProfile);
        return Ok(recruiter);
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(200, Type = typeof(Recruiter))]
    public IActionResult SignUp(RecruiterSignupRequest signUpRequest)
    {
        var recruiter = _recruiterRepository.Signup(signUpRequest);
        return Ok(recruiter);
    }
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(RecruiterLoginResponse))]
    public IActionResult Login(RecruiterLoginRequest loginRequest)
    {
        var recruiterLoginResponse = _recruiterRepository.Login(loginRequest);
        return Ok(recruiterLoginResponse);
    }
}