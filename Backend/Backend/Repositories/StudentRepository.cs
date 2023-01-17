using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;
    public StudentRepository(DataContext _context, IOptions<AppSettings> appSettings)
    {
        this._context = _context;
        _appSettings = appSettings.Value;
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

    public bool StudentExists(int studentId)
    {
        return _context.Students.Any(c => c.Id == studentId);
    }

    public Student CreateStudent(StudentDto studentDto)
    {
        var course = _context.Courses.Where(e => e.Id == 2).FirstOrDefault();
        
        Console.WriteLine("Student DTO:  " + studentDto.CityName);
        var cityByName = _context.Cities.Where(e => e.Name == studentDto.CityName).FirstOrDefault();
        var collegeByName = _context.Colleges.Where(e => e.Name == studentDto.CollegeName).FirstOrDefault();
        string someUrl = "https://cdn-icons-png.flaticon.com/512/149/149071.png";
        using (var webClient = new WebClient())
        {
            byte[] imageBytes = webClient.DownloadData(someUrl);
            var student = new Student()
            {
                Name = studentDto.Name,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Password = studentDto.Password
            };
            
            Console.WriteLine("COLLEGE: " + collegeByName.Name + " " + collegeByName.Id + " " + collegeByName.FoundationDate);
            if (cityByName != null && collegeByName != null)
            {
                Console.WriteLine("CITY: " + cityByName.Name + " " + cityByName.Id);
                student.EnrollDate = studentDto.EnrollDate;
                student.DateOfBirth = studentDto.DateOfBirth;
                student.Address = studentDto.Address;
                student.Languages = studentDto.Languages;
                student.Skills = studentDto.Skills;
                student.Phone = studentDto.Phone;
                student.Department = studentDto.Department;
                student.Image = imageBytes;
                student.IsActivated = studentDto.IsActivated;
                student.City = cityByName;
                student.College = collegeByName;
                // cityByName.Colleges.Add(college);
                _context.Students.Add(student);
            }
            else
            {
                Console.WriteLine("City Not Found");
                _context.Students.Add(student);
            
            }
            _context.SaveChanges();
            return student;
        }

        
    }

    public Student UpdateStudent(int studentId, StudentDto studentDto)
    {
        var cityByName = _context.Cities.Where(e => e.Name == studentDto.CityName).FirstOrDefault();
        var collegeByName = _context.Colleges.Where(e => e.Name == studentDto.CollegeName).FirstOrDefault();
        var student = this.GetStudent(studentId);
        if (cityByName != null)
        {
            student.City = cityByName;
        }

        if (collegeByName != null)
        {
            student.College = collegeByName;
        }

        student.Name = studentDto.Name;
        student.LastName = studentDto.LastName;
        student.EnrollDate = studentDto.EnrollDate;
        student.DateOfBirth = studentDto.DateOfBirth;
        student.Address = studentDto.Address;
        student.Languages = studentDto.Languages;
        student.Skills = studentDto.Skills;
        student.Phone = studentDto.Phone;
        student.Department = studentDto.Department;
        student.IsActivated = studentDto.IsActivated;
        
        
        
        _context.Students.Update(student);
        _context.SaveChanges();
        return student;
    }

    public void DeleteStudent(int studentId)
    {
        _context.Students.Remove(this.GetStudent(studentId));
        _context.SaveChanges();
    }

    public Student AddCourses(int studentId, AddCourseToStudent addCourseToStudent)
    {
        var student = this.GetStudent(studentId);
        var courses = new List<StudentCourse>();
        foreach (var c in addCourseToStudent.Courses)
        {
            var courseByName = _context.Courses.Where(e => e.Name == c.Name).FirstOrDefault();
            var studentCourse = new StudentCourse()
            {
                Student = student,
                Course = courseByName
            };
            courses.Add(studentCourse);
        }
        _context.StudentCourses.AddRange(courses);
        _context.SaveChanges();
        
        return this.GetStudent(studentId);

    }

    public Student UpdateStudentProfile(int studentId, StudentUpdateProfile studentUpdateProfile)
    {
        var cityByName = _context.Cities.Where(e => e.Name == studentUpdateProfile.CityName).FirstOrDefault();
        var collegeByName = _context.Colleges.Where(e => e.Name == studentUpdateProfile.CollegeName).FirstOrDefault();
        var student = this.GetStudent(studentId);
        if (cityByName != null)
        {
            student.City = cityByName;
        }

        if (collegeByName != null)
        {
            student.College = collegeByName;
        }
        
        student.EnrollDate = studentUpdateProfile.EnrollDate;
        student.DateOfBirth = studentUpdateProfile.DateOfBirth;
        student.Address = studentUpdateProfile.Address;
        student.Skills = studentUpdateProfile.Skills;
        student.Phone = studentUpdateProfile.Phone;
        student.Department = studentUpdateProfile.Department;
        // student.Image = studentUpdateProfile.Image,
        student.IsActivated = false;
        
        
        
        _context.Students.Update(student);
        _context.SaveChanges();
        return student;
    }

    public Student Signup(StudentSignUpRequest signUpRequest)
    {
        string someUrl = "https://cdn-icons-png.flaticon.com/512/149/149071.png";
        using (var webClient = new WebClient())
        {
            
            byte[] imageBytes = webClient.DownloadData(someUrl);
            var student = new Student()
            {
                Name = signUpRequest.Name,
                LastName = signUpRequest.LastName,
                Email = signUpRequest.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(signUpRequest.Password),
                Image = imageBytes,
                IsActivated = false
            };
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

    }
    
    public StudentLoginReponse Login(StudentLoginRequest loginRequest)
    {
        var student =
            _context.Students.Where(s => s.Email == loginRequest.Email).FirstOrDefault();
        Console.WriteLine("Password1: " + student.Password);
        Console.WriteLine("Password2: " +  BCrypt.Net.BCrypt.HashPassword(loginRequest.Password));
        
        if (student != null)
        {
            bool verify = BCrypt.Net.BCrypt.Verify(loginRequest.Password, student.Password);
            if (verify)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, student.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                student.Token = tokenHandler.WriteToken(token);
                _context.Students.Update(student);
                _context.SaveChanges();
                return new StudentLoginReponse()
                {
                    Key = student.Token,
                    Message = "Login successful"
                };    
            }

            return new StudentLoginReponse()
            {
                Key = null,
                Message = "Password is not true"
            };

        }

        return new StudentLoginReponse()
        {
            Key = null,
            Message = "Email or password is not true"
        };
    }
}