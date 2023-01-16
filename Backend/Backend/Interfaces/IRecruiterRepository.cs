using Backend.Models;

namespace Backend.Interfaces;

public interface IRecruiterRepository
{
    ICollection<Recruiter> GetRecruiters();
    Recruiter GetRecruiter(int id);
    Recruiter GetRecruiterCity(int city);
    ICollection<Company> GetCompanies(int id);
}