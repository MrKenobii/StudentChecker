using AutoMapper;
using Backend.DataTransferObject.Admin;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly IMapper _mapper;
    public AdminController(IAdminRepository adminRepository, IMapper mapper)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Admin>))]
    public IActionResult GetAdmins()
    {
        var admins = _adminRepository.GetAdmins();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(admins);
    }

    [HttpGet("{adminId}")]
    [ProducesResponseType(200, Type = typeof(Admin))]
    [ProducesResponseType(400)]
    public IActionResult GetAdminById(int adminId)
    {
        if (!_adminRepository.AdminExists(adminId))
            return NotFound();
        var admin = _adminRepository.GetAdminById(adminId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(admin);
    }
    [HttpGet("token/{token}")]
    [ProducesResponseType(200, Type = typeof(Admin))]
    [ProducesResponseType(400)]
    public IActionResult GetAdminByToken(string token)
    {
        var admin = _adminRepository.GetAdminByToken(token);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(admin);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(AdminPostResponse))]
    public IActionResult CreateAdmin([FromBody] AdminPostRequest adminPostRequest)
    {
        var admin = _adminRepository.CreateAdmin(adminPostRequest);
        return Created("HttpStatusCode.Created",admin);
    }
    
    [HttpPut("{adminId}")]
    [ProducesResponseType(200, Type = typeof(AdminPostResponse))]
    public IActionResult UpdateStudent(int adminId, [FromBody] AdminPostRequest adminPostRequest)
    {
        if (!_adminRepository.AdminExists(adminId))
            return NotFound();
        var admin = _adminRepository.UpdateAdmin(adminId, adminPostRequest);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(admin);
    }
    [HttpDelete("{adminId}")]
    [ProducesResponseType(200, Type = typeof(AdminDeleteResponse))]
    public IActionResult DeleteStudent(int adminId)
    {
        var adminDeleteResponse = _adminRepository.DeleteAdmin(adminId);
        return Ok(adminDeleteResponse);
    }
    
    [HttpPost("login")]
    [ProducesResponseType(200 , Type = typeof(AdminLoginResponse))]
    public IActionResult Login(AdminLoginRequest loginRequest)
    {
        AdminLoginResponse adminLogin = _adminRepository.Login(loginRequest);
        if (adminLogin == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        return Ok(adminLogin);
    }
    [HttpPost("activate-student/{studentId}")]
    [ProducesResponseType(200 , Type = typeof(ActivateStudentResponse))]
    public IActionResult ActivateStudent(int studentId)
    {
        ActivateStudentResponse activatedStudent = _adminRepository.ActivateStudent(studentId);
        return Ok(activatedStudent);
    }
    [HttpPost("activate-recruiter/{recruiterId}")]
    [ProducesResponseType(200 , Type = typeof(ActivateRecruiterResponse))]
    public IActionResult ActivateRecruiter(int recruiterId)
    {
        ActivateRecruiterResponse activatedRecruiter = _adminRepository.ActivateRecruiter(recruiterId);
        return Ok(activatedRecruiter);
    }
    
}