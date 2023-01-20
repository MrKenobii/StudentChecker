using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICourseRepository
{
    ICollection<Course> GetCourses();
    Course GetCourse(int id);
    ICollection<StudentDto> GetStudents(int id);
    bool CourseExists(int id);
    CoursePostResponse CreateCourse(CoursePostRequest courseDto);
    void DeleteCourse(int courseId);
    CoursePostResponse UpdateCourse(int courseId, CoursePostRequest courseDto);
}