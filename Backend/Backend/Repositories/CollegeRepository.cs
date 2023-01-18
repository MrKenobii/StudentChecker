using System.Runtime.CompilerServices;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly DataContext _context;
    // private readonly ICityRepository _cityRepository;

    // public CollegeRepository(ICityRepository cityRepository, DataContext dataContext)
    // {
    //     this._context = dataContext;
    //     this._cityRepository = cityRepository;
    // }
    public CollegeRepository(DataContext dataContext)
    {
        this._context = dataContext;
    }
    public ICollection<College> GetColleges()
    {
        return _context.Colleges.ToList();
    }

    public College GetCollege(int id)
    {
        return _context.Colleges.Where(c => c.Id == id).FirstOrDefault();
    }

    public CityDto GetCityByCollege(int collegeId)
    {
        var college = this.GetCollege(collegeId);
        Console.WriteLine("College Name " + college.Name);
        var city = _context.Companies.Where(o => o.Id == collegeId).Select(c => c.City).FirstOrDefault();
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

    public ICollection<Student> GetStudentsByCollege(int id)
    {
        return _context.Students.Where(c => c.College.Id == id).ToList();
    }

    public bool CollegeExists(int collegeId)
    {
        return _context.Colleges.Any(c => c.Id == collegeId);
    }

    public College CreateCollege(CollegeDto collegeDto)
    {
        Console.WriteLine("Colllege DTO:  " + collegeDto.CityName);
        var cityByName = _context.Cities.Where(e => e.Name == collegeDto.CityName).FirstOrDefault();
        // var cityByName = _cityRepository.GetCityByName(collegeDto.Name);
        var college = new College()
        {
            Name = collegeDto.Name,
            FoundationDate = collegeDto.FoundationDate,
            EmailExtension = collegeDto.EmailExtension
        };
        Console.WriteLine("COLLEGE: " + college.Name + " " + college.Id + " " + college.FoundationDate);
        if (cityByName != null)
        {
            Console.WriteLine("CITY: " + cityByName.Name + " " + cityByName.Id);
            college.Name = collegeDto.Name;
            college.FoundationDate = collegeDto.FoundationDate;
            college.City = cityByName;
            // cityByName.Colleges.Add(college);
            _context.Colleges.Add(college);
        }
        else
        {
            Console.WriteLine("City Not Found");
            _context.Colleges.Add(college);
            
        }
        _context.SaveChanges();
        return college;
        
    }

    public College UpdateCollege(int collegeId, CollegeDto collegeDto)
    {
        var cityByName = _context.Cities.Where(e => e.Name == collegeDto.CityName).FirstOrDefault();
        var college = this.GetCollege(collegeId);
        
        if (collegeDto.Name != null)
        {
            college.Name = collegeDto.Name;    
        }

        if (collegeDto.FoundationDate != null)
        {
            college.FoundationDate = collegeDto.FoundationDate;
        }

        if (collegeDto.CityName != null)
        {
            college.City = cityByName;
        }

        if (collegeDto.EmailExtension != null)
        {
            college.EmailExtension = collegeDto.EmailExtension;
        }
        
        
        _context.Colleges.Update(college);
        _context.SaveChanges();
        return college;
    }

    public void DeleteCollege(int collegeId)
    {
        _context.Colleges.Remove(this.GetCollege(collegeId));
        _context.SaveChanges();
    }
}