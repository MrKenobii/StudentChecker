using Backend.Models;

namespace Backend.DataTransferObject;

public class AddCourseToStudent  
{
    public IEnumerable<CoursePostRequest> Courses { get; set; }
    
}