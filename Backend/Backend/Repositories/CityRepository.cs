using Backend.Data;
using Backend.DataTransferObject;
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
    public City GetCityByName(string collegeName)
    {
        return _context.Cities.Where(e => e.Name == collegeName).FirstOrDefault();
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

    public City CreateCity(CityDto cityDto)
    {
        var city = new City()
        {
            Name = cityDto.Name
        };
        Console.WriteLine(city);
        _context.Cities.Add(city);
        _context.SaveChanges();
        return city;
    }

    public City UpdateCity(int cityId, CityDto cityDto)
    {
        var city = this.GetCity(cityId);
        city.Name = cityDto.Name;
        _context.Cities.Update(city);
        _context.SaveChanges();
        return city;
    }

    public void DeleteCity(int cityId)
    {
        _context.Cities.Remove(this.GetCity(cityId));
        _context.SaveChanges();
    }

    
}