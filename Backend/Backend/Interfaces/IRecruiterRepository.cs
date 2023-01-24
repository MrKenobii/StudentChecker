using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Models;

namespace Backend.Interfaces;

public interface IRecruiterRepository
{
    ICollection<Recruiter> GetRecruiters();
    Recruiter GetRecruiter(int id);
    ICollection<CompanyDto> GetCompanies(int id);
    void DeleteRecruiter(int recruiterId);
    RecruiterPostResponse UpdateRecruiter(int recruiterId, RecruiterPostRequest recruiterDto);
    RecruiterPostResponse CreateRecruiter(RecruiterPostRequest recruiterDto);
    bool RecruiterExists(int recruiterId);
    Recruiter UpdateRecruiterProfile(int recruiterId, RecruiterUpdateProfile recruiterUpdateProfile);
    RecruiterSignUpResponse Signup(RecruiterSignupRequest signUpRequest);
    RecruiterLoginResponse Login(RecruiterLoginRequest loginRequest);
    RecruiterDto GetRecruiterByToken(string token);
    RecruiterTokenGetResponse GetTokenByRecruiterId(int recruiterId);
    RecruiterDto AddCompany(int recruiterId, AddCompanyToRecruiter addCompanyToRecruiter);
    EditProfileResponse EditProfile(int recruiterId, RecruiterEditProfileRequest recruiter);
}