using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly DataContext _context;

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

    public College GetCollegeByCity(int cityId)
    {
        return _context.Colleges.Where(o => o.City.Id == cityId).FirstOrDefault();
        //return _context.Cities.Where(e => e.Id == city).Select(c => c.Colleges).ToList();
        return null;
    }

    public ICollection<Student> GetStudentsByCollege(int id)
    {
        return _context.Students.Where(c => c.College.Id == id).ToList();
    }
}