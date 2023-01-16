using Backend.Data;
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

    public Company GetCompanyCity(int city)
    {
        return _context.Companies.Where(o => o.City.Id == city).FirstOrDefault();
    }

    public ICollection<Recruiter> GetRecruiters(int id)
    {
        return _context.Recruiters.Where(c => c.Id == id).ToList();
        //return _context.Cities.Where(o => o.Id == id).Select(c => c.).FirstOrDefault(); // One - Many
    }
}