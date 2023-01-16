using Backend.Models;

namespace Backend.Interfaces;

public interface IStudentRepository
{
    ICollection<Student> GetStudents();
    Student GetStudent(int id);
    Student GetStudentCity(int city);
    ICollection<Course> GetCourses(int id);
}