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
    [ProducesResponseType(200, Type = typeof(DeleteResponse))]
    public IActionResult DeleteRecruiter(int recruiterId)
    {
        var deleteResponse = _recruiterRepository.DeleteRecruiter(recruiterId);
        return Ok(deleteResponse);
    }
    [HttpPut("{recruiterId}/update-profile")]
    [ProducesResponseType(200, Type = typeof(Recruiter))]
    public IActionResult UpdateProfile(int recruiterId, [FromBody] RecruiterUpdateProfile recruiterUpdateProfile)
    {
        var recruiter = _recruiterRepository.UpdateRecruiterProfile(recruiterId, recruiterUpdateProfile);
        return Ok(recruiter);
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(200, Type = typeof(RecruiterSignUpResponse))]
    public IActionResult SignUp(RecruiterSignupRequest signUpRequest)
    {
        var recruiter = _recruiterRepository.Signup(signUpRequest);
        _recruiterRepository.SendEmail(signUpRequest);
        return Ok(recruiter);
    }
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(RecruiterLoginResponse))]
    public IActionResult Login(RecruiterLoginRequest loginRequest)
    {
        var recruiterLoginResponse = _recruiterRepository.Login(loginRequest);
        return Ok(recruiterLoginResponse);
    }
    [HttpGet("{token}/token")]
    [ProducesResponseType(200, Type = typeof(RecruiterDto))]
    public IActionResult GetRecruiterByToken(string token)
    {
        var recruiterByToken = _recruiterRepository.GetRecruiterByToken(token);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(recruiterByToken);
    }
    [HttpGet("token/{recruiterId}")]
    [ProducesResponseType(200, Type = typeof(RecruiterTokenGetResponse))]
    public IActionResult GetTokenByRecruiterId(int recruiterId)
    {
        if (!_recruiterRepository.RecruiterExists(recruiterId))
            return NotFound();
        var token = _recruiterRepository.GetTokenByRecruiterId(recruiterId);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(token);
    }
    
    [HttpPost("{recruiterId}/verify-account")]
    [ProducesResponseType(200, Type = typeof(RecruiterVerifyAccountResponse))]
    public IActionResult VerifyAccount(int recruiterId, RecruiterVerifyAccountRequest verifyAccountRequest)
    {
        var recruiterVerify = _recruiterRepository.VerifyAccount(recruiterId, verifyAccountRequest);
        return Ok(recruiterVerify);
    }
    [HttpPut("{recruiterId}/add-company")]
    [ProducesResponseType(200, Type = typeof(RecruiterDto))]
    public IActionResult AddCompanyToRecruiter(int recruiterId, AddCompanyToRecruiter company)
    {
        if (!_recruiterRepository.RecruiterExists(recruiterId))
            return NotFound();
        var recruiter = _recruiterRepository.AddCompany(recruiterId, company);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(recruiter);
    }
    [HttpPut("{recruiterId}/edit-profile")]
    [ProducesResponseType(200, Type = typeof(EditProfileResponse))]
    public IActionResult EditProfile(int recruiterId, RecruiterEditProfileRequest recruiter)
    {
        if (!_recruiterRepository.RecruiterExists(recruiterId))
            return NotFound();
        var message = _recruiterRepository.EditProfile(recruiterId, recruiter);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(message);
    }
    [HttpPut("{recruiterId}/change-password")]
    [ProducesResponseType(200, Type = typeof(ChangePasswordResponse))]
    public IActionResult ChangeRecruiterPassword(int recruiterId, ChangePasswordRequest request)
    {
        if (!_recruiterRepository.RecruiterExists(recruiterId))
            return NotFound();
        var message = _recruiterRepository.ChangePassword(recruiterId, request);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(message);
    }
}