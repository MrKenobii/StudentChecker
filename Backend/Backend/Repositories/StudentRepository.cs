using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DataContext _context;
    public StudentRepository(DataContext _context)
    {
        this._context = _context;
    }
    public ICollection<Student> GetStudents()
    {
        return _context.Students.ToList();
    }

    public Student GetStudent(int id)
    {
        return _context.Students.Where(e => e.Id == id).FirstOrDefault();
    }

    public Student GetStudentCity(int city)
    {
        return _context.Students.Where(o => o.City.Id == city).FirstOrDefault();
    }

    public ICollection<Course> GetCourses(int id)
    {
        return null;
        // return _context.Courses.Where(c => c.Student.Id == id).ToList();
    }
}