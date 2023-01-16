using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class RecruiterRepository : IRecruiterRepository
{
    private readonly DataContext _context;
    public RecruiterRepository(DataContext context)
    {
        this._context = context;
    }
    public ICollection<Recruiter> GetRecruiters()
    {
        return _context.Recruiters.ToList();
    }

    public Recruiter GetRecruiter(int id)
    {
        return _context.Recruiters.Where(c => c.Id == id).FirstOrDefault();
    }

    public Recruiter GetRecruiterCity(int city)
    {
        return null;
        // return _context.Recruiters.Where(o => o.REc.Id == city).FirstOrDefault();
    }

    public ICollection<Company> GetCompanies(int id)
    {
        return null;
        // return _context.Companies.Where(c => c.Re.Id == id).ToList();
    }
}