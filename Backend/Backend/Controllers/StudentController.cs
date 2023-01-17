using AutoMapper;
using Backend.DataTransferObject;
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
    [Authorize]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
    public IActionResult GetStudents()
    {
        var students = _studentRepository.GetStudents(); 
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(students);
    }
    [HttpGet("{studentId}")]
    [ProducesResponseType(200, Type = typeof(Student))]
    [ProducesResponseType(400)]
    public IActionResult GetStudentById(int studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var student = _mapper.Map<StudentDto>( _studentRepository.GetStudent((studentId)));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(student);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Student))]
    public IActionResult CreateStudent([FromBody] StudentDto studentDto)
    {
        var student = _mapper.Map<Student>(_studentRepository.CreateStudent(studentDto));
        return Created("HttpStatusCode.Created",student);
    }
    [HttpPut("{studentId}")]
    [ProducesResponseType(200, Type = typeof(Student))]
    public IActionResult UpdateStudent(int studentId, [FromBody] StudentDto studentDto)
    {
        var student = _mapper.Map<Student>(_studentRepository.UpdateStudent(studentId, studentDto));
        return Ok(student);
    }
    [HttpDelete("{studentId}")]
    [ProducesResponseType(200)]
    public void DeleteStudent(int studentId)
    {
        _studentRepository.DeleteStudent(studentId);
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

    [HttpPost("sign-up")]
    [ProducesResponseType(200, Type = typeof(Student))]
    public IActionResult SignUp(StudentSignUpRequest signUpRequest)
    {
        var student = _studentRepository.Signup(signUpRequest);
        return Ok(student);
    }
    [HttpPost("login")]
    [ProducesResponseType(200)]
    public IActionResult Login(StudentLoginRequest loginRequest)
    {
        var student = _studentRepository.Login(loginRequest);
        if (student == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        return Ok(student);
    }
}