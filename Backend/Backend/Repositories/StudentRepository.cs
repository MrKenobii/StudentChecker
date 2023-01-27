using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Interfaces;
using Backend.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using MySqlConnector;

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
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 588")]
    public ICollection<StudentDto> GetStudents()
    {
        var _students = new List<StudentDto>();
        var students = _context.Students.ToList();
        if (students.Count > 0)
        {
            foreach (var student in students)
            {
                _students.Add(new StudentDto()
                {
                    Address = student.Address,
                    CityName = this.GetStudentCity(student.Id).Name,
                    CollegeName = this.GetStudentCollege(student.Id).Name,
                    DateOfBirth = student.DateOfBirth,
                    Department = student.Department,
                    Email = student.Email,
                    EnrollDate = student.EnrollDate,
                    Id = student.Id,
                    Image = student.Image,
                    IsActivated = student.IsActivated,
                    Name = student.Name,
                    LastName = student.LastName,
                    Languages = student.Languages,
                    Skills = student.Skills,
                    Password = student.Password,
                    Phone = student.Phone
                });
            }

            return _students;
        }

        return new List<StudentDto>();

    }
    
    public Student GetStudent(int id)
    {
        return _context.Students.Where(e => e.Id == id).FirstOrDefault();
    }

    public StudentDto GetStudentById(int studentId)
    {
        var student = this.GetStudent(studentId);
        if (student != null)
        {
            return new StudentDto()
            {
                Address = student.Address,
                CityName = this.GetStudentCity(student.Id).Name,
                CollegeName = this.GetStudentCollege(student.Id).Name,
                DateOfBirth = student.DateOfBirth,
                Department = student.Department,
                Email = student.Email,
                EnrollDate = student.EnrollDate,
                Id = student.Id,
                Image = student.Image,
                IsActivated = student.IsActivated,
                Name = student.Name,
                LastName = student.LastName,
                Languages = student.Languages,
                Skills = student.Skills,
                Password = student.Password,
                Phone = student.Phone
            };
        }

        return new StudentDto();
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

    public StudentPostResponse CreateStudent(StudentPostRequest studentDto)
    {
        if (studentDto != null)
        {
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
                var coursePostRequests = new List<CoursePostRequest>();
                coursePostRequests.AddRange(studentDto.Courses);
                this.AddCourses(student.Id, new AddCourseToStudent()
                {
                    Courses = coursePostRequests
                });

                Console.WriteLine("COLLEGE: " + collegeByName.Name + " " + collegeByName.Id + " " +
                                  collegeByName.FoundationDate);
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
                    if (studentDto.Image != null)
                    {
                        student.Image = studentDto.Image;
                    }
                    else
                    {
                        student.Image = imageBytes;
                    }
                    student.IsActivated = studentDto.IsActivated;
                    student.City = cityByName;
                    student.College = collegeByName;

                    _context.Students.Add(student);
                    cityByName.Students.Add(student);
                    collegeByName.Students.Add(student);
                    _context.SaveChanges();
                    return new StudentPostResponse()
                    {
                        Email = student.Email,
                        Id = student.Id,
                        LastName = student.LastName,
                        Message = "Student " + student.Name + " " + student.LastName + " was successfully added",
                        Name = student.Name
                    };
                }
                return new StudentPostResponse()
                {
                    Email = null,
                    Id = 0,
                    LastName = null,
                    Message = "Something went wrong!!!",
                    Name = null
                };
            }
            
        }

        return new StudentPostResponse()
        {
            Email = null,
            Id = 0,
            LastName = null,
            Message = "Student is not added. Bad Request!!",
            Name = null
        };
    }

    public StudentPostResponse UpdateStudent(int studentId, StudentPostRequest studentDto)
    {
        if (studentDto != null)
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
                student.IsActivated = true;
                student.Password = BCrypt.Net.BCrypt.HashPassword(studentDto.Password);
                student.Image = studentDto.Image;
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, studentDto.Token),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                

                var token = tokenHandler.CreateToken(tokenDescriptor);
                student.Token = tokenHandler.WriteToken(token); 
                
                var courses = new List<StudentCourse>();
                ICollection<CourseDto> courseDtos = this.GetCourses(studentId);
                Console.WriteLine("Checkpoint-1");
                foreach (var courseDto in courseDtos)
                {
                    Console.WriteLine("Checkpoint-1 aa");
                    var courseByName = _context.Courses.Where(e => e.Name == courseDto.Name).FirstOrDefault();
                    var studentCourse = new StudentCourse()
                    {
                        Student = student,
                        Course = courseByName
                    };
                    if (studentCourse != null)
                    {
                        Console.WriteLine("Checkpoint-2");
                        courses.Add(studentCourse);
                    }
                }
                if (courses.Count > 0)
                {
                    foreach (var studentCourse in courses)
                    {
                        
                        Console.WriteLine("Checkpoint-3");
                        Console.WriteLine("Inside Removeee -------------------------> " +studentCourse.Course.Id  +  " " + studentCourse.Student.Id);

                        string connStr = "server=localhost;user=root;database=company_table;port=3306;password=root";
                        MySqlConnection conn = new MySqlConnection(connStr);
                        try
                        {
                            Console.WriteLine("Connecting to MySQL...");
                            conn.Open();
                            string sql = "DELETE FROM `student_checker_v2`.`StudentCourses` WHERE (`StudentId` = '" +
                                         studentCourse.Student.Id + "');";
                            Console.WriteLine("Query: " + sql);
                            MySqlConnection MyConn2 = new MySqlConnection(connStr);
                            MySqlCommand MyCommand2 = new MySqlCommand(sql, MyConn2);
                            MySqlDataReader MyReader2;
                            MyConn2.Open();
                            MyReader2 = MyCommand2.ExecuteReader();
                            while (MyReader2.Read())
                            {
                            }
            
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.ToString());
                        }

                        conn.Close();
                    }
                   
                }
                
                
                var _courses = new List<StudentCourse>();
                string courseString = "";
                foreach (var c in studentDto.Courses)
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
                        _courses.Add(studentCourse);    
                    }
            
                }

                if (_courses.Count > 0)
                {
                    _context.StudentCourses.AddRange(_courses);    
                }
                
                
                _context.Students.Update(student);
                _context.SaveChanges();
                return new StudentPostResponse()
                {
                    Email = student.Email,
                    Id = student.Id,
                    LastName = student.LastName,
                    Message = "Student " + student.Name + " " + student.LastName + " was successfully updated",
                    Name = student.Name
                };
            }
            return new StudentPostResponse()
            {
                Email = null,
                Id = 0,
                LastName = null,
                Message = "Something went wrong!!!",
                Name = null
            };    
        }
        return new StudentPostResponse()
        {
            Email = null,
            Id = 0,
            LastName = null,
            Message = "Student is not added. Bad Request!!",
            Name = null
        };
        
    }

    public DeleteResponse DeleteStudent(int studentId)
    {
        var student = this.GetStudent(studentId);
        _context.Students.Remove(student);
        _context.SaveChanges();
        return new DeleteResponse()
        {
            Message = "Student " + student.Name + " " + student.LastName + " was deleted"
        };
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
            // student.IsActivated = true;
        
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
                Id = student.Id,
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
            var studentByEmail = _context.Students.Where(s => s.Email == signUpRequest.Email).FirstOrDefault();
            if (studentByEmail == null)
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
                    Id = student.Id,
                    Name = student.Name,
                    LastName = student.LastName,
                    Email = student.Email,
                    Message = "Student "+student.Name+" "+student.LastName+" has successfully registered"
                };    
            }
            if (studentByEmail != null && !studentByEmail.IsActivated)
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
                _context.Students.Update(student);
                _context.SaveChanges();
                return new StudentResponse()
                {
                    Name = student.Name,
                    LastName = student.LastName,
                    Email = student.Email,
                    Message = "Email already taken check your mailbox and confirm verification code"
                };
            }
            
            return new StudentResponse()
            {
                Name = null,
                LastName = null,
                Email = null,
                Message = "Email : " + signUpRequest.Email + " has already been taken"
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
            email.Subject = "Registration for Student Application";
            var TagA = "<a target=\"_blank\" href=\"http://localhost:4200/activate/student/" + student.Id + "?token=" + verifyToken + "\" >Activate your Account</a>";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Dear Student "+student.Name +" "+student.LastName + "</h1>" +
                                                                "<h2>Thanks for registering.</h2> " +
                                                                "<h3>Your Verify Token: " + verifyToken+"</h3> " +
                                                                "<h4>You can directly visit the link below</h4>" + TagA
                                                                 };
        
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
                // student.IsActivated = true;
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

    
    public StudentDto GetStudentByKey(string key)
    {
        var student = _context.Students.Where(a => a.Token == key).FirstOrDefault();
        if (student != null)
        {
            Console.WriteLine(student.Name);
            return new StudentDto()
            {
                Address = student.Address,
                CityName = this.GetStudentCity(student.Id).Name,
                CollegeName = this.GetStudentCollege(student.Id).Name,
                DateOfBirth = student.DateOfBirth,
                Department = student.Department,
                EnrollDate = student.EnrollDate,
                Email = student.Email,
                Id = student.Id,
                Image = student.Image,
                IsActivated = student.IsActivated,
                Skills = student.Skills,
                Languages = student.Languages,
                Phone = student.Phone,
                Password = student.Password,
                Name = student.Name,
                LastName = student.LastName,
            };
        }
        Console.WriteLine("Student not found");
        return new StudentDto();
    }

    public StudentGetKeyResponse GetKeyByStudentId(int id)
    {
        var student = this.GetStudent(id);
        if (student != null && student.Token != null)
        {
            return new StudentGetKeyResponse()
            {
                Key = student.Token,
                Message = "Student's token successfully returned;"
            };
        }

        return new StudentGetKeyResponse()
        {
            Key = null,
            Message = "Student " + student.Name + " " + student.LastName + " has no active token"
        };
    }

    private void UpdateCourses()
    {
        
    }
    public StudentEditProfileResponse EditStudentProfile(int studentId, StudentEditProfile studentEditProfile)
    {
        if (studentEditProfile != null)
        {
            var student = this.GetStudent(studentId);
            if (student != null)
            {
                var city = _context.Cities.Where(s => s.Name == studentEditProfile.CityName).FirstOrDefault();
                var college = _context.Colleges.Where(s => s.Name == studentEditProfile.CollegeName).FirstOrDefault();
                
                var courses = new List<StudentCourse>();
                ICollection<CourseDto> courseDtos = this.GetCourses(studentId);
                Console.WriteLine("Checkpoint-1");
                foreach (var courseDto in courseDtos)
                {
                    Console.WriteLine("Checkpoint-1 aa");
                    var courseByName = _context.Courses.Where(e => e.Name == courseDto.Name).FirstOrDefault();
                    var studentCourse = new StudentCourse()
                    {
                        Student = student,
                        Course = courseByName
                    };
                    if (studentCourse != null)
                    {
                        Console.WriteLine("Checkpoint-2");
                        courses.Add(studentCourse);
                    }
                }
                if (courses.Count > 0)
                {
                    foreach (var studentCourse in courses)
                    {
                        
                        Console.WriteLine("Checkpoint-3");
                        Console.WriteLine("Inside Removeee -------------------------> " +studentCourse.Course.Id  +  " " + studentCourse.Student.Id);

                        string connStr = "server=localhost;user=root;database=company_table;port=3306;password=root";
                        MySqlConnection conn = new MySqlConnection(connStr);
                        try
                        {
                            Console.WriteLine("Connecting to MySQL...");
                            conn.Open();
                            string sql = "DELETE FROM `student_checker_v2`.`StudentCourses` WHERE (`StudentId` = '" +
                                         studentCourse.Student.Id + "')";
                            Console.WriteLine("Query: " + sql);
                            MySqlConnection MyConn2 = new MySqlConnection(connStr);
                            MySqlCommand MyCommand2 = new MySqlCommand(sql, MyConn2);
                            MySqlDataReader MyReader2;
                            MyConn2.Open();
                            MyReader2 = MyCommand2.ExecuteReader();
                            while (MyReader2.Read())
                            {
                            }
            
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.ToString());
                        }

                        conn.Close();
                        //_context.StudentCourses.Remove(studentCourse);
                        //_context.SaveChanges();     
                    }
                   
                }
                
                
                var _courses = new List<StudentCourse>();
                string courseString = "";
                foreach (var c in studentEditProfile.Courses)
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
                        _courses.Add(studentCourse);    
                    }
            
                }

                if (_courses.Count > 0)
                {
                    _context.StudentCourses.AddRange(_courses);    
                }
        
                // _context.SaveChanges();
                
                // Courses Department CityName CollegeName
                
                student.City = city;
                student.College = college;
                student.Address = studentEditProfile.Address;
                student.Phone = studentEditProfile.Phone;
                student.Name = studentEditProfile.Name;
                student.LastName = studentEditProfile.LastName;
                student.Email = studentEditProfile.Email;
                student.Skills = studentEditProfile.Skills;
                student.Languages = studentEditProfile.Languages;
                student.EnrollDate = studentEditProfile.EnrollDate;
                student.DateOfBirth = studentEditProfile.DateOfBirth;
                student.Image = studentEditProfile.Image;
                
                _context.Students.Update(student);
                _context.SaveChanges();
                
                
                
                return new StudentEditProfileResponse()
                {
                    Key = student.Token,
                    Message = "Student " + student.Name + " " + student.LastName +
                              "'s profile was successfully updated"
                };
            }
            return new StudentEditProfileResponse()
            {
                Key = null,
                Message = "Student with id: " + studentId + " was not found"
            };
        }
        return new StudentEditProfileResponse()
        {
            Key = null,
            Message = "Payload is not valid"
        };
    }

    public ChangePasswordResponse ChangePassword(int studentId, ChangePasswordRequest request)
    {
        var student = this.GetStudent(studentId);
        if (student != null)
        {
            bool verify = BCrypt.Net.BCrypt.Verify(request.PrevPassword, student.Password);
            if (verify)
            {
                if (request.NewPassword == request.NewPasswordCopy)
                {
                    student.Password = request.NewPassword;
                    _context.Students.Update(student);
                    _context.SaveChanges();
                    return new ChangePasswordResponse()
                    {
                        Message = "Student " + student.Name + " " + student.LastName +
                                  "'s password has updated successfully",
                        Status = true
                    };
                }

                return new ChangePasswordResponse()
                {
                    Message = "Passwords are not matching",
                    Status = false
                };
            }

            return new ChangePasswordResponse()
            {
                Message = "Your old Password is not correct",
                Status = false
            };
        }

        return new ChangePasswordResponse()
        {
            Message = "Student with id " + studentId + " was not found",
            Status = false
        };

    }
}