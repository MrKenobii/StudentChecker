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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace Backend.Repositories;

public class RecruiterRepository : IRecruiterRepository
{
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public RecruiterRepository(DataContext context, IOptions<AppSettings> appSettings)
    {
        this._context = context;
        _appSettings = appSettings.Value;
    }

    public ICollection<Recruiter> GetRecruiters()
    {
        return _context.Recruiters.ToList();
    }

    public Recruiter GetRecruiter(int id)
    {
        return _context.Recruiters.Where(c => c.Id == id).FirstOrDefault();
    }

    

    public ICollection<CompanyDto> GetCompanies(int id)
    {
        var recruiter = this.GetRecruiter(id);
        ICollection<CompanyDto> companies = new List<CompanyDto>();
        if (recruiter != null)
        {
            var recruiterCompanies = _context.RecruiterCompanies.Where(e => e.RecruiterId == id).ToList();
            Console.WriteLine("RecruiterCompanies Length: " + recruiterCompanies.Count);
            foreach (var recruiterCompany in recruiterCompanies)
            {
                Console.WriteLine("Companies Length: " + recruiterCompany.CompanyId);
                if (recruiterCompany.CompanyId != null)
                {
                    var firstOrDefault = _context.Companies.Where(e => e.Id == recruiterCompany.CompanyId).FirstOrDefault();
                    Console.WriteLine("Company Name: " + firstOrDefault.Name);
                    if (firstOrDefault != null)
                    {
                        // var college = _context.Students.Where(o => o.Id == firstOrDefault.Id).Select(c => c.College).FirstOrDefault();
                        var city = _context.Companies.Where(o => o.Id == firstOrDefault.Id).Select(c => c.City).FirstOrDefault();
                        companies.Add(new CompanyDto()
                        {
                            Name = firstOrDefault.Name,
                            Address = firstOrDefault.Address,
                            CityName = city.Name,
                            Email = firstOrDefault.Email,
                            Id = firstOrDefault.Id,
                            Phone = firstOrDefault.Phone,
                            CompanyType = firstOrDefault.CompanyType,
                            FoundationDate = firstOrDefault.FoundationDate,
                            CompanyKey = firstOrDefault.CompanyKey
                        });    
                    }
                    
                }
            }
            
            return companies;
        }
        return new List<CompanyDto>();
    }

    public DeleteResponse DeleteRecruiter(int recruiterId)
    {
        var recruiter = this.GetRecruiter(recruiterId);
        _context.Recruiters.Remove(recruiter);
        _context.SaveChanges();
        return new DeleteResponse()
        {
            Message = "Recruiter " + recruiter.Name + " " + recruiter.LastName + " was deleted"
        };
    }

    public RecruiterPostResponse UpdateRecruiter(int recruiterId, RecruiterPostRequest recruiterDto)
    {
        var recruiter = this.GetRecruiter(recruiterId);
        if (recruiter != null)
        {
            recruiter.Name = recruiterDto.Name;
            recruiter.LastName = recruiterDto.LastName;
            recruiter.Email = recruiterDto.Email;
            recruiter.Password = recruiterDto.Password;
            recruiter.HireDate = recruiterDto.HireDate;
            recruiter.DateOfBirth = recruiterDto.DateOfBirth;
            recruiter.Address = recruiterDto.Address;
            recruiter.Phone = recruiterDto.Phone;
            recruiter.IsActivated = recruiterDto.IsActivated;



            _context.Recruiters.Update(recruiter);
            _context.SaveChanges();
            return new RecruiterPostResponse()
            {
                Email = recruiter.Email,
                Id = recruiter.Id,
                LastName = recruiter.LastName,
                Message = "Recruiter " +recruiter.Name +" " +recruiter.LastName + " was updated successfully",
                Name = recruiter.Name
            };    
        }
        return new RecruiterPostResponse()
        {
            Email = null,
            Id = recruiterId,
            LastName = null,
            Message = "Recruiter " +recruiterId+" was not found",
            Name = null
        };
    }

    public RecruiterPostResponse CreateRecruiter(RecruiterPostRequest recruiterDto)
    {
        if (recruiterDto != null)
        {
            Console.WriteLine("Recruiter DTO:  " + recruiterDto.Name); 
            string someUrl =
                "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/271deea8-e28c-41a3-aaf5-2913f5f48be6/de7834s-6515bd40-8b2c-4dc6-a843-5ac1a95a8b55.jpg?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzI3MWRlZWE4LWUyOGMtNDFhMy1hYWY1LTI5MTNmNWY0OGJlNlwvZGU3ODM0cy02NTE1YmQ0MC04YjJjLTRkYzYtYTg0My01YWMxYTk1YThiNTUuanBnIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.BopkDn1ptIwbmcKHdAOlYHyAOOACXW0Zfgbs0-6BY-E";
            using (var webClient = new WebClient())
            {
                byte[] imageBytes = webClient.DownloadData(someUrl);
                var recruiter = new Recruiter()
                {
                    Name = recruiterDto.Name,
                    LastName = recruiterDto.LastName,
                    Email = recruiterDto.Email,
                    Password = recruiterDto.Password,
                    HireDate = recruiterDto.HireDate,
                    DateOfBirth = recruiterDto.DateOfBirth,
                    Address = recruiterDto.Address,
                    Phone = recruiterDto.Phone,
                    IsActivated = recruiterDto.IsActivated,
                    Image = imageBytes
                };
                _context.Recruiters.Add(recruiter);

                _context.SaveChanges();
                return new RecruiterPostResponse()
                {
                    Name = recruiterDto.Name,
                    LastName = recruiterDto.LastName,
                    Email = recruiterDto.Email,
                    Message = "Recruiter " +recruiter.Name +" " +recruiter.LastName + " was updated successfully",
                };
            }
        }

        return new RecruiterPostResponse()
        {
            Email = null,
            Id = 0,
            LastName = null,
            Message = "Something went wrong",
            Name = null
        };

    }

    public bool RecruiterExists(int recruiterId)
    {
        return _context.Recruiters.Any(c => c.Id == recruiterId);
    }

    public Recruiter UpdateRecruiterProfile(int recruiterId, RecruiterUpdateProfile recruiterUpdateProfile)
    {
        var recruiter = this.GetRecruiter(recruiterId);

        recruiter.Address = recruiterUpdateProfile.Address;
        recruiter.HireDate = recruiterUpdateProfile.HireDate;
        recruiter.DateOfBirth = recruiterUpdateProfile.DateOfBirth;
        recruiter.Phone = recruiterUpdateProfile.Phone;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, recruiter.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        recruiter.Token = tokenHandler.WriteToken(token);

        // student.Image = studentUpdateProfile.Image,
        recruiter.IsActivated = false;



        _context.Recruiters.Update(recruiter);
        _context.SaveChanges();
        return recruiter;
    }
    

    public RecruiterSignUpResponse Signup(RecruiterSignupRequest signUpRequest)
    {
        string someUrl =
            "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/271deea8-e28c-41a3-aaf5-2913f5f48be6/de7834s-6515bd40-8b2c-4dc6-a843-5ac1a95a8b55.jpg?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzI3MWRlZWE4LWUyOGMtNDFhMy1hYWY1LTI5MTNmNWY0OGJlNlwvZGU3ODM0cy02NTE1YmQ0MC04YjJjLTRkYzYtYTg0My01YWMxYTk1YThiNTUuanBnIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.BopkDn1ptIwbmcKHdAOlYHyAOOACXW0Zfgbs0-6BY-E";
        using (var webClient = new WebClient())
        {
            var recruiterByEmail = _context.Recruiters.Where(s => s.Email == signUpRequest.Email).FirstOrDefault();
            if (recruiterByEmail == null)
            {
                byte[] imageBytes = webClient.DownloadData(someUrl);
                var recruiter = new Recruiter()
                {
                    Name = signUpRequest.Name,
                    LastName = signUpRequest.LastName,
                    Email = signUpRequest.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(signUpRequest.Password),
                    Image = imageBytes,
                    IsActivated = false
                };
                _context.Recruiters.Add(recruiter);
                _context.SaveChanges();
                return new RecruiterSignUpResponse()
                {
                    Id = recruiter.Id,
                    Message = "Recruiter " +recruiter.Name +" " +recruiter.LastName+" has successfully registered",
                    Name = recruiter.Name,
                    LastName = recruiter.LastName,
                    Email = recruiter.Email
                };
            }

            if (recruiterByEmail != null && !recruiterByEmail.IsActivated)
            {
                byte[] imageBytes = webClient.DownloadData(someUrl);
                var recruiter = new Recruiter()
                {
                    Name = signUpRequest.Name,
                    LastName = signUpRequest.LastName,
                    Email = signUpRequest.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(signUpRequest.Password),
                    Image = imageBytes,
                    IsActivated = false
                };
                _context.Recruiters.Update(recruiter);
                _context.SaveChanges();
                return new RecruiterSignUpResponse()
                {
                    Id = recruiter.Id,
                    Name = recruiter.Name,
                    LastName = recruiter.LastName,
                    Email = recruiter.Email,
                    Message = "Email already taken check your mailbox and confirm verification code"
                };
            }
            return new RecruiterSignUpResponse()
            {
                Id = 0,
                Name = null,
                LastName = null,
                Email = null,
                Message = "Bad Request"
            };
            
        }
    }
    public string SendEmail(RecruiterSignupRequest signUpRequest)
    {
        var recruiter = _context.Recruiters.Where(e => e.Email == signUpRequest.Email).FirstOrDefault();
        
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[8];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var verifyToken = new String(stringChars);
        if (recruiter != null)
        {
            recruiter.VerifyToken = verifyToken;
            _context.Recruiters.Update(recruiter);
            _context.SaveChanges();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("arely93@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(recruiter.Email));
            email.Subject = "Registration for Application";
            string TagA = "<a  target=\"_blank\" href=\"http://localhost:4200/activate/recruiter/" + recruiter.Id + "?token=" + verifyToken + "\">Activate your Account</a>";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Dear Recruiter "+recruiter.Name +" "+recruiter.LastName + "</h1>" +
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

        return "Recruiter "+recruiter.Name+" "+recruiter.LastName+" was not found";

    }

    public RecruiterVerifyAccountResponse VerifyAccount(int recruiterId, RecruiterVerifyAccountRequest verifyAccountRequest)
    { 
        var recruiter = this.GetRecruiter(recruiterId);
        if (recruiter != null)
        {
            if (verifyAccountRequest.VerifyToken == recruiter.VerifyToken)
            {
                // recruiter.IsActivated = true;
                recruiter.VerifyToken = null;

                _context.Recruiters.Update(recruiter);
                _context.SaveChanges();
                return new RecruiterVerifyAccountResponse()
                {
                    Message = "Account verification successful",
                    Email = recruiter.Email,
                    Name = recruiter.Name
                };
            }
            return new RecruiterVerifyAccountResponse()
            {
                Message = "Invalid token",
                Email = null,
                Name = null
            };
        }
        return new RecruiterVerifyAccountResponse()
        {
            Message = "Recruiter with id : " + recruiterId + " does not found",
            Email = null,
            Name = null
        };
    }

    public RecruiterLoginResponse Login(RecruiterLoginRequest loginRequest)
    {
        var recruiter = _context.Recruiters.Where(e => e.Email == loginRequest.Email)
            .FirstOrDefault();
        if (recruiter != null)
        {
            bool verify = BCrypt.Net.BCrypt.Verify(loginRequest.Password, recruiter.Password);
            if (verify)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, recruiter.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                recruiter.Token = tokenHandler.WriteToken(token);
                _context.Recruiters.Update(recruiter);
                _context.SaveChanges();
                return new RecruiterLoginResponse()
                {
                    Key = recruiter.Token,
                    Message = "Login Successful"
                };
            }
                
            return new RecruiterLoginResponse()
            {
                Key = null,
                Message = "Password is not true"
            };;
        }
        // Recruiter Does not Found
        return new RecruiterLoginResponse()
        {
            Key = null,
            Message = "Email or Password is not true"
        };
    }

    public RecruiterDto GetRecruiterByToken(string token)
    {
        var recruiter = _context.Recruiters.Where(r => r.Token == token).FirstOrDefault();
        if (recruiter != null)
        {
            return new RecruiterDto()
            {
                Address = recruiter.Address,
                DateOfBirth = recruiter.DateOfBirth,
                Email = recruiter.Email,
                HireDate = recruiter.HireDate,
                Id = recruiter.Id,
                IsActivated = recruiter.IsActivated,
                LastName = recruiter.LastName,
                Name = recruiter.Name,
                Password = recruiter.Password,
                Phone = recruiter.Phone,
                Image = recruiter.Image
            };
        }
        else return new RecruiterDto();
    }

    public RecruiterTokenGetResponse GetTokenByRecruiterId(int recruiterId)
    {
        var recruiter = this.GetRecruiter(recruiterId);
        if (recruiterId != null && recruiter.Token != null)
        {
            return new RecruiterTokenGetResponse()
            {
                Key = recruiter.Token,
                Message = "Recruiter's token successfully returned;"
            };
        }

        return new RecruiterTokenGetResponse()
        {
            Key = null,
            Message = "Recruiter  " + recruiter.Name + " " + recruiter.LastName + " has no active token"
        };
    }

    public RecruiterDto AddCompany(int recruiterId, AddCompanyToRecruiter addCompanyToRecruiter)
    {
        var recruiter = this.GetRecruiter(recruiterId);
        var companies = new List<RecruiterCompany>();
        foreach (var c in addCompanyToRecruiter.Companies)
        {
            var companyByName = _context.Companies.Where(e => e.Id == c.Id).FirstOrDefault();
            var recruiterCompany = new RecruiterCompany()
            {
                Company = companyByName,
                Recruiter = recruiter
            };
            companies.Add(recruiterCompany);
        }
        _context.RecruiterCompanies.AddRange(companies);
        _context.SaveChanges();
        return new RecruiterDto()
        {
            Address = recruiter.Address,
            DateOfBirth = recruiter.DateOfBirth,
            Email = recruiter.Email,
            HireDate = recruiter.HireDate,
            Id = recruiter.Id,
            IsActivated = recruiter.IsActivated,
            LastName = recruiter.LastName,
            Name = recruiter.Name,
            Password = recruiter.Password,
            Phone = recruiter.Phone,
            Image = recruiter.Image
        };

    }

    public EditProfileResponse EditProfile(int recruiterId, RecruiterEditProfileRequest recruiter)
    {
        var _recruiter = this.GetRecruiter(recruiterId);
        if (_recruiter != null)
        {
            var company = _context.Companies.Where(s => s.Name == recruiter.CompanyName).FirstOrDefault();
            Console.WriteLine("Company: " + company.Name);
            var enumerable = new List<CompanyRequestById>();
            enumerable.Add(new CompanyRequestById()
            {
                Id = company.Id
            });
            ICollection<CompanyDto> companyDtos = this.GetCompanies(recruiterId);
            Console.WriteLine("Length: " + companyDtos.Count);
            bool flag = false;
            foreach (var companyDto in companyDtos)
            {
                Console.WriteLine(companyDto.Id + " " + companyDto.Name + " " + company.Id);
                if (companyDto.Id == company.Id)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                Console.WriteLine("HEREEE");
                this.AddCompany(recruiterId, new AddCompanyToRecruiter()
                {
                    Companies = enumerable
                });
            }

            _recruiter.Address = recruiter.Address;
            _recruiter.HireDate = recruiter.HireDate;
            _recruiter.DateOfBirth = recruiter.DateOfBirth;
            _recruiter.Phone = recruiter.Phone;
            _recruiter.Name = recruiter.Name;
            _recruiter.LastName = recruiter.LastName;
            _recruiter.Email = recruiter.Email;
            _recruiter.Image = recruiter.Image;
    
            
            _context.Recruiters.Update(_recruiter);
            _context.SaveChanges();
            return new EditProfileResponse()
            {
                Message = "Recruiter " + _recruiter.Name + " " + _recruiter.LastName +
                          "' s profile has successfully updated"
            };
        }

        return new EditProfileResponse() { Message = "Something went wrong!!" };
    }

    public ChangePasswordResponse ChangePassword(int recruiterId, ChangePasswordRequest request)
    {
        var recruiter = this.GetRecruiter(recruiterId);
        if (recruiter != null)
        {
            bool verify = BCrypt.Net.BCrypt.Verify(request.PrevPassword, recruiter.Password);
            if (verify)
            {
                if (request.NewPassword == request.NewPasswordCopy)
                {
                    recruiter.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                    _context.Recruiters.Update(recruiter);
                    _context.SaveChanges();
                    return new ChangePasswordResponse()
                    {
                        Message = "Recruiter " + recruiter.Name + " " + recruiter.LastName +
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
            Message = "Recruiter with id " + recruiterId + " was not found",
            Status = false
        };

    }
}