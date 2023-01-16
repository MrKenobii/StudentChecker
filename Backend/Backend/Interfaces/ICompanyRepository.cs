using Backend.Models;

namespace Backend.Interfaces;

public interface ICompanyRepository
{
    ICollection<Company> GetCompanies();
    Company GetCompany(int id);
    Company GetCompanyCity(int city);
    ICollection<Recruiter> GetRecruiters(int id);
}