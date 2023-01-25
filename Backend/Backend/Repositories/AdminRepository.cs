using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DataTransferObject.Admin;
using Backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
}