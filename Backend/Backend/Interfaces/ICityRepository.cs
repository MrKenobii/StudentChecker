using Backend.Models;

namespace Backend.Interfaces;

public interface ICityRepository
{
    ICollection<City> GetCities();
    City GetCity(int id);
    bool CityExists(int id);
    ICollection<Company> GetCompaniesByCity(int id);
    ICollection<College> GetCollegesByCity(int id);
    ICollection<Student> GetStudentsByCity(int id);
}