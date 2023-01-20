using AutoMapper;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers;

[Route("/[controller]")]
[ApiController]
public class CourseController : Controller
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseController(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CourseDto>))]
    public IActionResult GetCourses()
    {
        var courses = _mapper.Map<List<CourseDto>>(_courseRepository.GetCourses());
        //var courses = _courseRepository.GetCourses();
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
        var course = _mapper.Map<CourseDto>( _courseRepository.GetCourse((courseId)));
        //var course = _courseRepository.GetCourse(courseId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(course);
    }
    [HttpGet("student/{courseId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<StudentDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetStudentByCourse(int courseId)
    { 
        var students = _courseRepository.GetStudents(courseId);
        //var students = _courseRepository.GetStudents(courseId);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(students);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CoursePostResponse))]
    public IActionResult CreateCourse([FromBody] CoursePostRequest courseDto)
    {
        // Console.WriteLine(courseDto.Name);
        var course = _courseRepository.CreateCourse(courseDto);
        return Created("HttpStatusCode.Created",course);
        //return Ok(_courseRepository.CreateCourse(courseDto));
    }

    [HttpDelete("{courseId}")]
    [ProducesResponseType(200)]
    public void DeleteCourse(int courseId)
    {
         Console.WriteLine(courseId);
        _courseRepository.DeleteCourse(courseId);
    }

    [HttpPut("{courseId}")]
    [ProducesResponseType(201, Type = typeof(CourseDto))]
    public IActionResult UpdateCourse(int courseId, [FromBody] CoursePostRequest courseDto)
    {
        var course = _courseRepository.UpdateCourse(courseId, courseDto);
        return Ok(course);
    }
}