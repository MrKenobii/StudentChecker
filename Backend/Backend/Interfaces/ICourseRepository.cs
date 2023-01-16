using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICourseRepository
{
    ICollection<Course> GetCourses();
    Course GetCourse(int id);
    Company GetCompanyCity(int city);
    ICollection<Student> GetStudents(int id);
    bool CourseExists(int id);
    string CreateCourse(CourseDto courseDto);
    void DeleteCourse(int courseId);
    Course? UpdateCourse(int courseId, CourseDto courseDto);
}