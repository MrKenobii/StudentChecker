using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AutoMapper;
using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentController(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }
    [HttpGet]
    // [Authorize]
    [ProducesResponseType(200, Type = typeof(IEnumerable<StudentDto>))]
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 130")]
    public IActionResult GetStudents()
    {
        var students = _studentRepository.GetStudents(); 
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(students);
    }
    [HttpGet("{studentId}")]
    [ProducesResponseType(200, Type = typeof(StudentDto))]
    [ProducesResponseType(400)]
    public IActionResult GetStudentById(int studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var student =  _studentRepository.GetStudentById(studentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(student);
    }
    [HttpGet("get-random/{studentId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<StudentRandomResponse>))]
    [ProducesResponseType(400)]
    public IActionResult GetRandomStudentsById(int studentId)
    {
        var students =  _studentRepository.GetRandomStudents(studentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(students);
    }
    [HttpGet("{studentId}/courses")]
    [ProducesResponseType(200, Type=typeof(IEnumerable<CourseDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCoursesByStudentId(int studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var collection = _studentRepository.GetCourses(studentId);
        return Ok(collection);
    }
    [HttpGet("{studentId}/city")]
    [ProducesResponseType(200, Type=typeof(IEnumerable<CityDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCityByStudentId(int studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var city = _studentRepository.GetStudentCity(studentId);
        return Ok(city);
    }
    [HttpGet("{studentId}/college")]
    [ProducesResponseType(200, Type=typeof(IEnumerable<CollegeDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCollegeByStudentId(int studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var college = _studentRepository.GetStudentCollege(studentId);
        return Ok(college);
    }
    [HttpGet("key/{key}")]
    [ProducesResponseType(200, Type=typeof(IEnumerable<StudentDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetStudentByKey(string key)
    {
        // if (!_studentRepository.StudentExists(studentId))
        //     return NotFound();
        StudentDto student = _studentRepository.GetStudentByKey(key);
        return Ok(student);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(StudentPostResponse))]
    public IActionResult CreateStudent([FromBody] StudentPostRequest studentDto)
    {
        var student = _studentRepository.CreateStudent(studentDto);
        return Created("HttpStatusCode.Created",student);
    }
    [HttpPut("{studentId}")]
    [ProducesResponseType(200, Type = typeof(StudentPostResponse))]
    public IActionResult UpdateStudent(int studentId, [FromBody] StudentPostRequest studentDto)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var student = _studentRepository.UpdateStudent(studentId, studentDto);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(student);
    }
    [HttpDelete("{studentId}")]
    [ProducesResponseType(200, Type = typeof(DeleteResponse))]
    public IActionResult DeleteStudent(int studentId)
    {
        var deleteResponse = _studentRepository.DeleteStudent(studentId);
        return Ok(deleteResponse);
    }
    [HttpPut("{studentId}/add-course")]
    [ProducesResponseType(200, Type = typeof(Student))]
    public IActionResult AddCourseStudent(int studentId, [FromBody] AddCourseToStudent addCourseToStudent)
    {
        var student = _studentRepository.AddCourses(studentId, addCourseToStudent);
        return Ok(student);
    }
    [HttpPut("{studentId}/update-profile")]
    [ProducesResponseType(200, Type = typeof(Student))]
    public IActionResult UpdateProfile(int studentId, [FromBody] StudentUpdateProfile studentUpdateProfile)
    {
        var student = _studentRepository.UpdateStudentProfile(studentId, studentUpdateProfile);
        return Ok(student);
    }
    [HttpPut("{studentId}/edit-profile")]
    [ProducesResponseType(200, Type = typeof(StudentEditProfileResponse))]
    public IActionResult EditProfile(int studentId, [FromBody] StudentEditProfile studentEditProfile)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var student = _studentRepository.EditStudentProfile(studentId, studentEditProfile);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(student);
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(200, Type = typeof(StudentResponse))]
    public IActionResult SignUp(StudentSignUpRequest signUpRequest)
    {
        var student = _studentRepository.Signup(signUpRequest);
        _studentRepository.SendEmail(signUpRequest);
        return Ok(student);
    }
    [HttpPost("login")]
    [ProducesResponseType(200 , Type = typeof(StudentLoginReponse))]
    public IActionResult Login(StudentLoginRequest loginRequest)
    {
        StudentLoginReponse studentLogin = _studentRepository.Login(loginRequest);
        if (studentLogin == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        return Ok(studentLogin);
    }

    [HttpPost("{studentId}/verify-account")]
    [ProducesResponseType(200, Type = typeof(StudentVerifyAccountResponse))]
    public IActionResult VerifyAccount(int studentId, StudentVerifyAccountRequest verifyAccountRequest)
    {
        var studentVerify = _studentRepository.VerifyAccount(studentId, verifyAccountRequest);
        return Ok(studentVerify);
    }

    [HttpGet("{studentId}/token")]
    [ProducesResponseType(200, Type = typeof(StudentGetKeyResponse))]
    public IActionResult GetTokenByStudentId(int studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var studentGetKeyResponse = _studentRepository.GetKeyByStudentId(studentId);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(studentGetKeyResponse);
    }
    [HttpPut("{studentId}/change-password")]
    [ProducesResponseType(200, Type = typeof(ChangePasswordResponse))]
    public IActionResult ChangeStudentPassword(int studentId, ChangePasswordRequest request)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var message = _studentRepository.ChangePassword(studentId, request);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(message);
    }
}