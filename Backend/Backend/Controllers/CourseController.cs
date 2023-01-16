using AutoMapper;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers;

public class CourseController : Controller
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseController(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    [HttpGet("/")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Course>))]
    public IActionResult GetCourses()
    {
        // var courses = _mapper.Map<List<CourseDto>>(_courseRepository.GetCourses());
        var courses = _courseRepository.GetCourses();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(courses);
    }
    [HttpGet("{courseId}")]
    [ProducesResponseType(200, Type = typeof(Course))]
    [ProducesResponseType(400)]
    public IActionResult GetCourse(int courseId)
    {
        if (!_courseRepository.CourseExists(courseId))
            return NotFound();
        // var course = _mapper.Map<CourseDto>( _courseRepository.GetCourse((courseId)));
        var course = _courseRepository.GetCourse(courseId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(course);
    }
    [HttpGet("student/{courseId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
    [ProducesResponseType(400)]
    public IActionResult GetStudentByCourse(int courseId)
    {
        // var students = _mapper.Map<List<StudentDto>>(_courseRepository.GetStudents(courseId));
        var students = _courseRepository.GetStudents(courseId);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(students);
    }

    [HttpPost("/")]
    [ProducesResponseType(201, Type = typeof(Course))]
    public IActionResult CreateCourse([FromBody] CourseDto courseDto)
    {
        // Console.WriteLine(courseDto.Name);
        return Ok(_courseRepository.CreateCourse(courseDto));
    }

    [HttpDelete("{courseId}")]
    [ProducesResponseType(200)]
    public void DeleteCourse(int courseId)
    {
         Console.WriteLine(courseId);
        _courseRepository.DeleteCourse(courseId);
    }

    [HttpPut("{courseId}")]
    [ProducesResponseType(201, Type = typeof(Course))]
    public IActionResult UpdateCourse(int courseId, [FromBody] CourseDto courseDto)
    {
        // Console.WriteLine(courseDto.Name);
        Console.WriteLine("Course Id: " + courseId);
        Console.WriteLine("Name: " + courseDto.Name);
        return Ok(_courseRepository.UpdateCourse(courseId, courseDto));
    }
}