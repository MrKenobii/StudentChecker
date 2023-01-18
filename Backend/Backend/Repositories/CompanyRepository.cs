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
    public ICollection<Company> GetCompanies()
    {
        return _context.Companies.ToList();
    }

    public Company GetCompany(int id)
    {
        return _context.Companies.Where(c => c.Id == id).FirstOrDefault();
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

    public ICollection<Recruiter> GetRecruiters(int id)
    {
        return _context.Recruiters.Where(c => c.Id == id).ToList();
        //return _context.Cities.Where(o => o.Id == id).Select(c => c.).FirstOrDefault(); // One - Many
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

    public Company UpdateCompany(int companyId, CompanyDto companyDto)
    {
        Console.WriteLine("COMPANYDTO : " + companyDto.Name + " " + companyDto.Id + " " + companyDto.FoundationDate);
        var cityByName = _context.Cities.Where(e => e.Name == companyDto.CityName).FirstOrDefault();
        var company = this.GetCompany(companyId);
        Console.WriteLine("COMPANY: " + company.Name + " " + company.Id + " " + company.FoundationDate);
        if (companyDto.Name != null)
        {
            company.Name = companyDto.Name;    
        }

        if (companyDto.CompanyKey != null)
        {
            company.CompanyKey = companyDto.CompanyKey;   
        }

        if (companyDto.CityName != null)
        {
            company.City = cityByName;
        }

        if (companyDto.FoundationDate != null)
        {
            company.FoundationDate = companyDto.FoundationDate;
        }
        if (companyDto.Address != null)
        {
            company.Address = companyDto.Address;
        }
        if (companyDto.CompanyType != null)
        {
            company.CompanyType = companyDto.CompanyType;
        }
        if (companyDto.Phone != null)
        {
            company.Phone = companyDto.Phone;
        }
        if (companyDto.Email != null)
        {
            company.Email = companyDto.Email;
        }
        
        _context.Companies.Update(company);
        _context.SaveChanges();
        return company;
    }

    public Company CreateCompany(CompanyDto companyDto)
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
            // cityByName.Colleges.Add(college);
            _context.Companies.Add(company);
        }
        else
        {
            Console.WriteLine("City Not Found");
            _context.Companies.Add(company);
            
        }
        _context.SaveChanges();
        return company;
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