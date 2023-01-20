using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICompanyRepository
{
    ICollection<CompanyGetResponse> GetCompanies();
    Company GetCompany(int id);

    CompanyGetResponse GetCompanyById(int companyId);
    CityDto GetCompanyCity(int city);
    ICollection<RecruiterDto> GetRecruitersByCompanyId(int id);
    bool CompanyExists(int companyId);
    void DeleteCompany(int companyId);
    CompanyDto UpdateCompany(int companyId, CompanyPostRequest companyDto);
    CompanyDto CreateCompany(CompanyPostRequest companyDto);
    Company AddRecruiter(int companyId, AddRecruiterToCompany addRecruiterToCompany);
}