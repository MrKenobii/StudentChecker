using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

    public Recruiter GetRecruiterCity(int city)
    {
        return null;
        // return _context.Recruiters.Where(o => o.REc.Id == city).FirstOrDefault();
    }

    public ICollection<Company> GetCompanies(int id)
    {
        return null;
        // return _context.Companies.Where(c => c.Re.Id == id).ToList();
    }

    public void DeleteRecruiter(int recruiterId)
    {
        _context.Recruiters.Remove(this.GetRecruiter(recruiterId));
        _context.SaveChanges();
    }

    public Recruiter UpdateRecruiter(int recruiterId, RecruiterDto recruiterDto)
    {
        var recruiter = this.GetRecruiter(recruiterId);

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
        return recruiter;
    }

    public Recruiter CreateRecruiter(RecruiterDto recruiterDto)
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
            return recruiter;
        }

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

        // student.Image = studentUpdateProfile.Image,
        recruiter.IsActivated = false;



        _context.Recruiters.Update(recruiter);
        _context.SaveChanges();
        return recruiter;
    }

    public Recruiter Signup(RecruiterSignupRequest signUpRequest)
    {
        string someUrl =
            "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/271deea8-e28c-41a3-aaf5-2913f5f48be6/de7834s-6515bd40-8b2c-4dc6-a843-5ac1a95a8b55.jpg?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzI3MWRlZWE4LWUyOGMtNDFhMy1hYWY1LTI5MTNmNWY0OGJlNlwvZGU3ODM0cy02NTE1YmQ0MC04YjJjLTRkYzYtYTg0My01YWMxYTk1YThiNTUuanBnIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.BopkDn1ptIwbmcKHdAOlYHyAOOACXW0Zfgbs0-6BY-E";
        using (var webClient = new WebClient())
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
            return recruiter;
        }
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
                    Message = "Login Successfull"
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

    public Recruiter AddCompany(int recruiterId, AddCompanyToRecruiter addCompanyToRecruiter)
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
        
        return this.GetRecruiter(recruiterId);
    }
}