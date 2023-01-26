using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DataTransferObject.Admin;
using Backend.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace Backend.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;
    public AdminRepository(DataContext _context,  IOptions<AppSettings> appSettings)
    {
        this._context = _context;
        _appSettings = appSettings.Value;
    }
    public ICollection<Admin> GetAdmins()
    {
        return _context.Admins.ToList();
    }
    public Admin GetAdminById(int adminId)
    {
        return _context.Admins.Where(a => a.Id == adminId).FirstOrDefault();
    }

    public bool AdminExists(int adminId)
    {
        return _context.Admins.Any(c => c.Id == adminId);
    }

    public AdminPostResponse CreateAdmin(AdminPostRequest adminPostRequest)
    {
        if (adminPostRequest != null)
        {
            var admin = new Admin()
            {
                Email = adminPostRequest.Email,
                Name = adminPostRequest.Name,
                LastName = adminPostRequest.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(adminPostRequest.Password),
                Token = null
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, admin.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            admin.Token = tokenHandler.WriteToken(token);
            
            _context.Admins.Add(admin);
            _context.SaveChanges();
            return new AdminPostResponse()
            {
                Message = "Admin has successfully created",
                Name = admin.Name,
                LastName = admin.LastName,
                Token = admin.Token
            };
        }

        return new AdminPostResponse()
        {
            Name = null,
            LastName = null,
            Message = "Something Went Wrong!!!",
            Token = null
        };
    }

    public AdminPostResponse UpdateAdmin(int adminId, AdminPostRequest adminPostRequest)
    {
        if (adminPostRequest != null)
        {
            var adminById = this.GetAdminById(adminId);
            if (adminById != null)
            {
                adminById.Email = adminPostRequest.Email;
                adminById.Name = adminPostRequest.Name;
                adminById.LastName = adminPostRequest.LastName;
                adminById.Password = BCrypt.Net.BCrypt.HashPassword(adminPostRequest.Password);
                _context.Admins.Update(adminById);
                _context.SaveChanges();
                return new AdminPostResponse()
                {
                    Message = "Admin has successfully updated",
                    Name = adminById.Name,
                    LastName = adminById.LastName,
                    Token = adminById.Token
                };
            }

            return new AdminPostResponse()
            {
                Name = null,
                LastName = null,
                Message = "Admin id: "+adminId+" not Exists!!!",
                Token = null
            };
        }
        return new AdminPostResponse()
        {
            Name = null,
            LastName = null,
            Message = "Something Went Wrong!!!",
            Token = null
        };
    }

    public AdminLoginResponse Login(AdminLoginRequest loginRequest)
    {
        var admin = _context.Admins.Where(a => a.Email == loginRequest.Email).FirstOrDefault();
        if (admin != null)
        {
            bool verify = BCrypt.Net.BCrypt.Verify(loginRequest.Password, admin.Password);
            if (verify)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, admin.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                admin.Token = tokenHandler.WriteToken(token);
                _context.Admins.Update(admin);
                _context.SaveChanges();
                return new AdminLoginResponse()
                {
                    Key = admin.Token,
                    Message = "Welcome Admin, " + admin.Name + " " + admin.LastName
                };
            }

            return new AdminLoginResponse()
            {
                Message = "Password is incorrect",
                Key = null
            };
        }

        return new AdminLoginResponse()
        {
            Key = null,
            Message = "Login was not successful"
        };
    }

    

    public AdminDeleteResponse DeleteAdmin(int adminId)
    {
        var admin = this.GetAdminById(adminId);
        _context.Admins.Remove(admin);
        _context.SaveChanges();
        return new AdminDeleteResponse()
        {
            Message = "Admin " + admin.Name + " " + admin.LastName + " was deleted"
        };
    }

    public Admin GetAdminByToken(string token)
    {
        Console.WriteLine(token);
        var firstOrDefault = _context.Admins.Where(a => a.Id == 1).FirstOrDefault();
        Console.WriteLine(firstOrDefault.Token);
        var admin = _context.Admins.Where(a => a.Token == token).FirstOrDefault();
        if (admin != null)
        {
            Console.WriteLine("ADMIN NULL NOT");
            return admin;
        }
        Console.WriteLine("ADMIN NULL !!!!");
        return new Admin();
    }

    public ActivateStudentResponse ActivateStudent(int studentId)
    {
        var student = _context.Students.Where(s => s.Id == studentId).FirstOrDefault();
        if (student != null)
        {
            if (!student.IsActivated)
            {
                student.IsActivated = true;
                _context.Students.Update(student);
                _context.SaveChanges();
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("arely93@ethereal.email"));
                email.To.Add(MailboxAddress.Parse(student.Email));
                email.Subject = "Registration for Student Application has been confirmed";
                var TagA = "<a href=\"http://localhost:4200/complete-profile/" + student.Id +"\" target=\"_blank\">Complete your Profile</a>";
                Console.WriteLine(TagA);
                email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Dear Student "+student.Name +" "+student.LastName + "</h1>" +
                                                                    "<h2>Your account is activated.</h2> " +
                                                                    "<h4>You can directly visit the link below</h4>" + TagA
                };
        
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                // smtp.Connect("smtp.live.com", 587, SecureSocketOptions.StartTls);
        
                smtp.Authenticate("arely93@ethereal.email", "uA8ZvSH3Q4EAb7xkqw");
                smtp.Send(email);
                smtp.Disconnect(true);
                return new ActivateStudentResponse()
                {
                    Message = "Student with " + student.Name + student.LastName + " has been confirmed",
                    Email = student.Email,
                    Id = student.Id,
                    LastName = student.LastName,
                    Name = student.Name
                };
                
            }

            return new ActivateStudentResponse()
            {
                Message = "Student is already activated",
                Email = null,
                Id = 0,
                LastName = null,
                Name = null
            };
        }

        return new ActivateStudentResponse()
        {
            Message = "Student with id: " + studentId + " was not found",
            Email = null,
            Id = 0,
            LastName = null,
            Name = null
        };
    }

    public ActivateRecruiterResponse ActivateRecruiter(int recruiterId)
    {
        var recruiter = _context.Recruiters.Where(s => s.Id == recruiterId).FirstOrDefault();
        if (recruiter != null)
        {
            if (!recruiter.IsActivated)
            {
                recruiter.IsActivated = true;
                _context.Recruiters.Update(recruiter);
                _context.SaveChanges();
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("arely93@ethereal.email"));
                email.To.Add(MailboxAddress.Parse(recruiter.Email));
                email.Subject = "Registration for Recruiter Application has been confirmed";
                var TagA = "<a href=\"http://localhost:4200/recruiter/complete-profile/" + recruiter.Id +"\" target=\"_blank\">Complete your Profile</a>";
                Console.WriteLine(TagA);
                email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Dear Recruiter "+recruiter.Name +" "+recruiter.LastName + "</h1>" +
                                                                    "<h2>Your account is activated.</h2> " +
                                                                    "<h4>You can directly visit the link below</h4>" + TagA
                };
        
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                // smtp.Connect("smtp.live.com", 587, SecureSocketOptions.StartTls);
        
                smtp.Authenticate("arely93@ethereal.email", "uA8ZvSH3Q4EAb7xkqw");
                smtp.Send(email);
                smtp.Disconnect(true);
                return new ActivateRecruiterResponse()
                {
                    Message = "Recruiter with " + recruiter.Name + recruiter.LastName + " has been confirmed",
                    Email = recruiter.Email,
                    Id = recruiter.Id,
                    LastName = recruiter.LastName,
                    Name = recruiter.Name
                };
                
            }

            return new ActivateRecruiterResponse()
            {
                Message = "Recruiter is already activated",
                Email = null,
                Id = 0,
                LastName = null,
                Name = null
            };
        }

        return new ActivateRecruiterResponse()
        {
            Message = "Recruiter with id: " + recruiterId + " was not found",
            Email = null,
            Id = 0,
            LastName = null,
            Name = null
        };
    }
}