using Backend.DataTransferObject.Admin;
using Backend.Models;

namespace Backend.Repositories;

public interface IAdminRepository
{
    ICollection<Admin> GetAdmins();
    bool AdminExists(int adminId);
    AdminPostResponse CreateAdmin(AdminPostRequest adminPostRequest);
    AdminPostResponse UpdateAdmin(int adminId, AdminPostRequest adminPostRequest);
    
    AdminLoginResponse Login(AdminLoginRequest loginRequest);
    Admin GetAdminById(int adminId);
    AdminDeleteResponse DeleteAdmin(int adminId);
    Admin GetAdminByToken(string token);
}