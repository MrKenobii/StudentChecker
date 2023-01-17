using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICompanyRepository
{
    ICollection<Company> GetCompanies();
    Company GetCompany(int id);
    Company GetCompanyCity(int city);
    ICollection<Recruiter> GetRecruiters(int id);
    bool CompanyExists(int companyId);
    void DeleteCompany(int companyId);
    Company UpdateCompany(int companyId, CompanyDto companyDto);
    Company CreateCompany(CompanyDto companyDto);
    Company AddRecruiter(int companyId, AddRecruiterToCompany addRecruiterToCompany);
}