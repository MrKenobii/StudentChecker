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
    

    public ICollection<StudentDto> GetStudents(int id)
    {
        var course = this.GetCourse(id);
        ICollection<StudentDto> courses = new List<StudentDto>();
        if (course != null)
        {
            var studentCourses = _context.StudentCourses.Where(e => e.CourseId == id).ToList();
            Console.WriteLine("Student Length: " + studentCourses.Count);
            foreach (var studentCourse in studentCourses)
            {
                Console.WriteLine("Course Length: " + studentCourse.StudentId);
                if (studentCourse.StudentId != null)
                {
                    var firstOrDefault = _context.Students.Where(e => e.Id == studentCourse.StudentId).FirstOrDefault();
                    Console.WriteLine("Student Name: " + firstOrDefault.Name);
                    if (firstOrDefault != null)
                    {
                        var college = _context.Students.Where(o => o.Id == firstOrDefault.Id).Select(c => c.College).FirstOrDefault();
                        var city = _context.Students.Where(o => o.Id == firstOrDefault.Id).Select(c => c.City).FirstOrDefault();
                        courses.Add(new StudentDto()
                        {
                            Name = firstOrDefault.Name,
                            LastName = firstOrDefault.LastName,
                            Address = firstOrDefault.Address,
                            CityName = city.Name,
                            CollegeName = college.Name,
                            DateOfBirth = firstOrDefault.DateOfBirth,
                            Department = firstOrDefault.Department,
                            Email = firstOrDefault.Email,
                            EnrollDate = firstOrDefault.EnrollDate,
                            Id = firstOrDefault.Id,
                            Image = firstOrDefault.Image,
                            IsActivated = firstOrDefault.IsActivated,
                            Languages = firstOrDefault.Languages,
                            Skills = firstOrDefault.Skills,
                            Phone = firstOrDefault.Phone,
                            Password = firstOrDefault.Password
                        });    
                    }
                    
                }
            }
            
            return courses;
        }
        return new List<StudentDto>();
    }

    public bool CourseExists(int courseId)
    {
        return _context.Courses.Any(c => c.Id == courseId);
    }

    public CoursePostResponse CreateCourse(CoursePostRequest courseDto)
    {
        if (courseDto != null)
        {
            var course = new Course()
            {
                Name = courseDto.Name
            };
        
            Console.WriteLine(course);
            _context.Courses.Add(course);
            _context.SaveChanges();
            
            return new CoursePostResponse()
            {
                Id = _context.Courses.Where(n => n.Name == course.Name).FirstOrDefault().Id,
                Name = course.Name,
                Message = "Course: " + course.Name+ " was successfully added"
                
            };    
        }

        return new CoursePostResponse()
        {
            Message = "Something went wrong",
            Id = 0,
            Name = null,
        };
    }

    public void DeleteCourse(int courseId)
    {
        _context.Courses.Remove(this.GetCourse(courseId));
        _context.SaveChanges();
    }

    public CoursePostResponse UpdateCourse(int courseId, CoursePostRequest courseDto)
    {
        Course course = this.GetCourse(courseId);
        if (courseDto != null)
        {

            if (courseDto.Name != null)
            {
                course.Name = courseDto.Name;
            }
        
            Console.WriteLine(course);
            _context.Courses.Update(course);
            _context.SaveChanges();
            
            return new CoursePostResponse()
            {
                Id = courseId,
                Name = course.Name,
                Message = "Course: " + course.Name+ " was successfully updated"
                
            };    
        }

        return new CoursePostResponse()
        {
            Message = "Something went wrong",
            Id = 0,
            Name = null,
        };
    }
}