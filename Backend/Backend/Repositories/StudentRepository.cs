using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

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

    public CityDto GetStudentCity(int studentId)
    {
        var city = _context.Students.Where(o => o.Id == studentId).Select(c => c.City).FirstOrDefault();
        if (city != null)
        {
            Console.WriteLine("City : " + city.Name);
            return new CityDto()
            {
                Name = city.Name,
                Id = city.Id
            };
        }

        return new CityDto()
        {

        };
    }

    public CollegeDto GetStudentCollege(int studentId)
    {
        var college = _context.Students.Where(o => o.Id == studentId).Select(c => c.College).FirstOrDefault();
        if (college != null)
        {
            Console.WriteLine("College : " + college.Name);
            var collegeCity = _context.Colleges.Where(c => c.Id == college.Id).Select(a => a.City).FirstOrDefault();
            return new CollegeDto()
            {
                Name = college.Name,
                Id = college.Id,
                FoundationDate = college.FoundationDate,
                CityName = collegeCity.Name,
                EmailExtension = college.EmailExtension
            };
        }

        return new CollegeDto()
        {

        };
    }

    public ICollection<CourseDto> GetCourses(int id)
    {
        var student = this.GetStudent(id);
        ICollection<CourseDto> courses = new List<CourseDto>();
        if (student != null)
        {
            var studentCourses = _context.StudentCourses.Where(e => e.StudentId == id).ToList();
            Console.WriteLine("Course Length: " + studentCourses.Count);
            foreach (var studentCourse in studentCourses)
            {
                Console.WriteLine("Course Length: " + studentCourse.CourseId);
                if (studentCourse.CourseId != null)
                {
                    var firstOrDefault = _context.Courses.Where(e => e.Id == studentCourse.CourseId).FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        CourseDto courseDto = new CourseDto()
                        {
                            Name = firstOrDefault.Name,
                            Id = firstOrDefault.Id
                        };
                        courses.Add(courseDto);    
                    }
                    
                    Console.WriteLine("Student " + studentCourse.Student.Id + " Course" + studentCourse.CourseId);
                }
            }
            
            return courses;
        }
        return new List<CourseDto>();
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
                
                _context.Students.Add(student);
                cityByName.Students.Add(student);
                collegeByName.Students.Add(student);
                // _context.Cities.Update(cityByName);
                // _context.Colleges.Update(collegeByName);
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

    public StudentResponse UpdateStudent(int studentId, StudentDto studentDto)
    {
        var cityByName = _context.Cities.Where(e => e.Name == studentDto.CityName).FirstOrDefault();
        var collegeByName = _context.Colleges.Where(e => e.Name == studentDto.CollegeName).FirstOrDefault();
        var student = this.GetStudent(studentId);
        if (student != null)
        {
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
            return new StudentResponse()
            {
                Email = student.Email,
                Name = student.Name,
                LastName = student.LastName,
                Message = "Student " + student.Name + " " + student.LastName +" was successfully updated"
            };
        }
        return new StudentResponse()
        {
            Email = null,
            Name = null,
            LastName = null,
            Message = "Student " + student.Name + " " + student.LastName +" was not found"
        };
        
    }

    public string DeleteStudent(int studentId)
    {
        var student = this.GetStudent(studentId);
        _context.Students.Remove(student);
        _context.SaveChanges();
        return "Student " + student.Name + " " + student.LastName + " was deleted";
    }

    public string AddCourses(int studentId, AddCourseToStudent addCourseToStudent)
    {
        var student = this.GetStudent(studentId);
        var courses = new List<StudentCourse>();
        string courseString = "";
        foreach (var c in addCourseToStudent.Courses)
        {
            var courseByName = _context.Courses.Where(e => e.Name == c.Name).FirstOrDefault();
            var studentCourse = new StudentCourse()
            {
                Student = student,
                Course = courseByName
            };
            if (studentCourse != null)
            {
                courseString = courseString +  c.Name +", " ;
                courses.Add(studentCourse);    
            }
            
        }

        if (courses.Count > 0)
        {
            _context.StudentCourses.AddRange(courses);    
        }
        
        _context.SaveChanges();
        return "Courses " + courseString + " was added to Student " + student.Name + " " + student.LastName;

        // return this.GetStudent(studentId);

    }

    public StudentResponse UpdateStudentProfile(int studentId, StudentUpdateProfile studentUpdateProfile)
    {
        var cityByName = _context.Cities.Where(e => e.Name == studentUpdateProfile.CityName).FirstOrDefault();
        var collegeByName = _context.Colleges.Where(e => e.Name == studentUpdateProfile.CollegeName).FirstOrDefault();
        var student = this.GetStudent(studentId);
        if (student != null)
        {
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
            student.Languages = studentUpdateProfile.Languages;
            student.Image = studentUpdateProfile.Image;
            student.IsActivated = true;
        
            var courses = new List<StudentCourse>();
            foreach (var c in studentUpdateProfile.Courses)
            {
                var courseByName = _context.Courses.Where(e => e.Name == c.Name).FirstOrDefault();
                var studentCourse = new StudentCourse()
                {
                    Student = student,
                    Course = courseByName
                };
                if (studentCourse != null)
                {
                    courses.Add(studentCourse);    
                }
            
            }

            if (courses.Count > 0)
            {
                _context.StudentCourses.AddRange(courses);    
            }
        
            // _context.SaveChanges();
        
            _context.Students.Update(student);
            _context.SaveChanges();
            return new StudentResponse()
            {
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Message = "Student "+student.Name+" "+student.LastName+" profile was successfully updated"
            };
        }
        return new StudentResponse()
        {
            Name =null,
            LastName = null,
            Email = null,
            Message = "Student "+student.Name+" "+student.LastName+" profile was not found"
        };
 
    }

    public StudentResponse Signup(StudentSignUpRequest signUpRequest)
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
            return new StudentResponse()
            {
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Message = "Student "+student.Name+" "+student.LastName+" has successfully registered"
            };
        }

    }
    
    public StudentLoginReponse Login(StudentLoginRequest loginRequest)
    {
        var student = _context.Students.Where(s => s.Email == loginRequest.Email).FirstOrDefault();
        
        if (student != null)
        {
            if (student.IsActivated)
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
                Message =
                    "Your account has not been enabled yet. Check your Email or wait for if you has just registered"
            };
        }

        return new StudentLoginReponse()
        {
            Key = null,
            Message = "Email or password is not true"
        };
    }

    public string SendEmail(StudentSignUpRequest signUpRequest)
    {
        var student = _context.Students.Where(e => e.Email == signUpRequest.Email).FirstOrDefault();
        
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[8];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var verifyToken = new String(stringChars);
        if (student != null)
        {
            student.VerifyToken = verifyToken;
            _context.Students.Update(student);
            _context.SaveChanges();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("arely93@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(student.Email));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Dear Student "+student.Name +" "+student.LastName +"</h1><h2>Thanks for registering.</h2> <h3>Your Verify Token: " + verifyToken+"</h3> " };
        
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            // smtp.Connect("smtp.live.com", 587, SecureSocketOptions.StartTls);
        
            smtp.Authenticate("arely93@ethereal.email", "uA8ZvSH3Q4EAb7xkqw");
            smtp.Send(email);
            smtp.Disconnect(true);
            return "Email was successfully send";
        }

        return "Student "+student.Name+" "+student.LastName+" was not found";

    }

    public StudentVerifyAccountResponse VerifyAccount(int studentId, StudentVerifyAccountRequest studentVerifyAccountRequest)
    {
        var student = this.GetStudent(studentId);
        if (student != null)
        {
            if (studentVerifyAccountRequest.VerifyToken == student.VerifyToken)
            {
                student.IsActivated = true;
                student.VerifyToken = null;

                _context.Students.Update(student);
                _context.SaveChanges();
                return new StudentVerifyAccountResponse()
                {
                    Message = "Account verification successful",
                    Email = student.Email,
                    Name = student.Name
                };
            }
            return new StudentVerifyAccountResponse()
            {
                Message = "Invalid token",
                Email = null,
                Name = null
            };
        }
        return new StudentVerifyAccountResponse()
        {
            Message = "Student with id : " + studentId + " does not found",
            Email = null,
            Name = null
        };
    }
}