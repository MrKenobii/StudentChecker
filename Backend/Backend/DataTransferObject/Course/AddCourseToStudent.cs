using Backend.Models;

namespace Backend.DataTransferObject;

public class AddCourseToStudent  
{
    public IEnumerable<CourseDto> Courses { get; set; }
    
}