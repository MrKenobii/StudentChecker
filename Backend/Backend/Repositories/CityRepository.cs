using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CityRepository : ICityRepository
{
    
    private readonly DataContext _context;
    public CityRepository(DataContext _context)
    {
        this._context = _context;
    }
    public ICollection<City> GetCities()
    {
        return _context.Cities.ToList();
    }

    public City GetCity(int id)
    {
        return _context.Cities.Where(e => e.Id == id).FirstOrDefault();
    }

    public bool CityExists(int id)
    {
        return _context.Cities.Any(c => c.Id == id);
    }

    public ICollection<Company> GetCompaniesByCity(int id)
    {
        return _context.Companies.Where(c => c.City.Id == id).ToList();
        
    }

    public ICollection<College> GetCollegesByCity(int id)
    {
        return _context.Colleges.Where(c => c.City.Id == id).ToList();
    }

    public ICollection<Student> GetStudentsByCity(int id)
    {
        return _context.Students.Where(c => c.City.Id == id).ToList();
    }
}