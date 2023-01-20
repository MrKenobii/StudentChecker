using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly DataContext _context;
    public CompanyRepository(DataContext context)
    {
        this._context = context;
    }
    public ICollection<CompanyGetResponse> GetCompanies()
    {
        var _companies = new List<CompanyGetResponse>();
        var companies = _context.Companies.ToList();
        if (companies.Count > 0)
        {
            foreach (var company in companies)
            {
                _companies.Add(new CompanyGetResponse()
                {
                    Address = company.Address,
                    City = this.GetCompanyCity(company.Id),
                    CompanyKey = company.CompanyKey,
                    CompanyType = company.CompanyType,
                    Email = company.Email,
                    FoundationDate = company.FoundationDate,
                    Id = company.Id,
                    Name = company.Name,
                    Phone = company.Phone
                });
            }

            return _companies;
        }

        return new List<CompanyGetResponse>();

    }
    
    public Company GetCompany(int id)
    {
        return _context.Companies.Where(c => c.Id == id).FirstOrDefault();
    }

    public CompanyGetResponse GetCompanyById(int companyId)
    {
        var company = this.GetCompany(companyId);
        if (company != null)
        {
            return new CompanyGetResponse()
            {
                Address = company.Address,
                City = this.GetCompanyCity(company.Id),
                CompanyKey = company.CompanyKey,
                CompanyType = company.CompanyType,
                Email = company.Email,
                FoundationDate = company.FoundationDate,
                Id = company.Id,
                Name = company.Name,
                Phone = company.Phone
            };    
        }

        return new CompanyGetResponse();

    }

    public CityDto GetCompanyCity(int companyId)
    {
        var company = this.GetCompany(companyId);
        Console.WriteLine("Company NAme " + company.Name);
        var city = _context.Companies.Where(o => o.Id == companyId).Select(c => c.City).FirstOrDefault();
        if (city != null)
        {
            Console.WriteLine("City : " + city.Name);
            return new CityDto()
            {
                Name = city.Name,
                Id = city.Id
            };
        }

        return new CityDto()
        {

        };
    }

    public ICollection<RecruiterDto> GetRecruitersByCompanyId(int id)
    {
        var company = this.GetCompany(id);
        ICollection<RecruiterDto> recruiters = new List<RecruiterDto>();
        if (company != null)
        {
            var recruiterCompanies = _context.RecruiterCompanies.Where(e => e.CompanyId == id).ToList();
            Console.WriteLine("RecruiterCompanies Length: " + recruiterCompanies.Count);
            foreach (var recruiterCompany in recruiterCompanies)
            {
                Console.WriteLine("Companies Length: " + recruiterCompany.CompanyId);
                if (recruiterCompany.RecruiterId != null)
                {
                    var firstOrDefault = _context.Recruiters.Where(e => e.Id == recruiterCompany.RecruiterId).FirstOrDefault();
                    Console.WriteLine("Recruiter Name: " + firstOrDefault.Name);
                    if (firstOrDefault != null)
                    {
                        // var college = _context.Students.Where(o => o.Id == firstOrDefault.Id).Select(c => c.College).FirstOrDefault();
                        var city = _context.Companies.Where(o => o.Id == firstOrDefault.Id).Select(c => c.City).FirstOrDefault();
                        recruiters.Add(new RecruiterDto()
                        {
                            Name = firstOrDefault.Name,
                            LastName = firstOrDefault.LastName,
                            DateOfBirth = (DateTime)firstOrDefault.DateOfBirth,
                            Password = firstOrDefault.Password,
                            HireDate = (DateTime)firstOrDefault.HireDate,
                            Address = firstOrDefault.Address,
                            Email = firstOrDefault.Email,
                            Id = firstOrDefault.Id,
                            Phone = firstOrDefault.Phone,
                            IsActivated = (bool)firstOrDefault.IsActivated,
                        });
                        
                    }
                    
                }
            }
            
            return recruiters;
        }
        return new List<RecruiterDto>();
        
        
    }

    public bool CompanyExists(int companyId)
    {
        return _context.Companies.Any(c => c.Id == companyId);
    }

    public void DeleteCompany(int companyId)
    {
        _context.Companies.Remove(this.GetCompany(companyId));
        _context.SaveChanges();
    }

    public CompanyDto UpdateCompany(int companyId, CompanyPostRequest companyDto)
    {
        Console.WriteLine("COMPANYDTO : " + companyDto.Name + " " + companyId + " " + companyDto.FoundationDate);
        var cityByName = _context.Cities.Where(e => e.Name == companyDto.CityName).FirstOrDefault();
        var company = this.GetCompany(companyId);
        Console.WriteLine("COMPANY: " + company.Name + " " + company.Id + " " + company.FoundationDate);
        if (companyDto != null && company != null)
        {
            company.Name = companyDto.Name;
            company.FoundationDate = companyDto.FoundationDate;
            company.Phone = companyDto.Phone;
            company.Address = companyDto.Address;
            company.CompanyKey = companyDto.CompanyKey;
            company.Email = companyDto.Email;
            company.CompanyType = companyDto.CompanyType;
            company.City = cityByName;
            _context.Companies.Update(company);
            _context.SaveChanges();
            return new  CompanyDto()
            {
                Name = companyDto.Name,
                FoundationDate = companyDto.FoundationDate,
                Phone = companyDto.Phone,
                Address = companyDto.Address,
                CompanyKey = companyDto.CompanyKey,
                Email = companyDto.Email,
                CompanyType = companyDto.CompanyType,
                CityName = cityByName.Name,
            };
        }
        return new CompanyDto();
    }

    public CompanyDto CreateCompany(CompanyPostRequest companyDto)
    {
        Console.WriteLine("Company DTO:  " + companyDto.CityName);
        var cityByName = _context.Cities.Where(e => e.Name == companyDto.CityName).FirstOrDefault();
        // var cityByName = _cityRepository.GetCityByName(collegeDto.Name);
        var company = new Company()
        {
            Name = companyDto.Name,
            FoundationDate = companyDto.FoundationDate,
            
        };
        Console.WriteLine("COMPANY: " + company.Name + " " + company.Id + " " + company.FoundationDate);
        if (cityByName != null)
        {
            Console.WriteLine("CITY: " + cityByName.Name + " " + cityByName.Id);
            company.Name = companyDto.Name;
            company.FoundationDate = companyDto.FoundationDate;
            company.Phone = companyDto.Phone;
            company.Address = companyDto.Address;
            company.CompanyKey = companyDto.CompanyKey;
            company.Email = companyDto.Email;
            company.CompanyType = companyDto.CompanyType;
            company.City = cityByName;
            _context.Companies.Add(company);
            _context.SaveChanges();
            return new CompanyDto()
            {
                Name = companyDto.Name,
                FoundationDate = companyDto.FoundationDate,
                Phone = companyDto.Phone,
                Address = companyDto.Address,
                CompanyKey = companyDto.CompanyKey,
                Email = companyDto.Email,
                CompanyType = companyDto.CompanyType,
                CityName = cityByName.Name,
            };
        }

        Console.WriteLine("City Not Found");
        
        return new CompanyDto();
    }

    public Company AddRecruiter(int companyId, AddRecruiterToCompany addRecruiterToCompany)
    {
        
        var company = this.GetCompany(companyId);
        var recruiters = new List<RecruiterCompany>();
        foreach (var c in addRecruiterToCompany.Recruiters)
        {
            var recruiterByName = _context.Recruiters.Where(e => e.Id == c.Id).FirstOrDefault();
            var studentCourse = new RecruiterCompany()
            {
                Company = company,
                Recruiter = recruiterByName
            };
            recruiters.Add(studentCourse);
        }
        _context.RecruiterCompanies.AddRange(recruiters);
        _context.SaveChanges();
        
        return this.GetCompany(companyId);
    }
}