using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly DataContext _context;
    public CourseRepository(DataContext _context)
    {
        this._context = _context;
    }
    
    public ICollection<Course> GetCourses()
    {
        return _context.Courses.ToList();
    }

    public Course GetCourse(int id)
    {
        return _context.Courses.Where(e => e.Id == id).FirstOrDefault();
    }

    public Company GetCompanyCity(int city)
    {
        return _context.Companies.Where(o => o.City.Id == city).FirstOrDefault();
    }

    public ICollection<Student> GetStudents(int id)
    {
        return null;
        // return _context.StudentCourses.Where(c => c.CourseId == id).ToList();
    }

    public bool CourseExists(int courseId)
    {
        return _context.Courses.Any(c => c.Id == courseId);
    }

    public Course CreateCourse(CourseDto courseDto)
    {
        var course = new Course()
        {
            Name = courseDto.Name
        };
        
        Console.WriteLine(course);
        _context.Courses.Add(course);
        _context.SaveChanges();
        return course;
    }

    public void DeleteCourse(int courseId)
    {
        _context.Courses.Remove(this.GetCourse(courseId));
        _context.SaveChanges();
    }

    public Course UpdateCourse(int courseId, CourseDto courseDto)
    {
        Course course = this.GetCourse(courseId);
        course.Name = courseDto.Name;
        Console.WriteLine("Course Name UpdateCourse() " + course.Name);
        _context.Update(course);
        _context.SaveChanges();
        return course;
    }
}