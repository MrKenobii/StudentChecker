using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Models;

namespace Backend.Interfaces;

public interface IRecruiterRepository
{
    ICollection<Recruiter> GetRecruiters();
    Recruiter GetRecruiter(int id);
    Recruiter GetRecruiterCity(int city);
    ICollection<Company> GetCompanies(int id);
    void DeleteRecruiter(int recruiterId);
    Recruiter UpdateRecruiter(int recruiterId, RecruiterDto recruiterDto);
    Recruiter CreateRecruiter(RecruiterDto recruiterDto);
    bool RecruiterExists(int recruiterId);
    Recruiter UpdateRecruiterProfile(int recruiterId, RecruiterUpdateProfile recruiterUpdateProfile);
    Recruiter Signup(RecruiterSignupRequest signUpRequest);
    RecruiterLoginResponse Login(RecruiterLoginRequest loginRequest);
}