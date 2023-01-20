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
    Recruiter Signup(RecruiterSignupRequest signUpRequest);
    RecruiterLoginResponse Login(RecruiterLoginRequest loginRequest);
}