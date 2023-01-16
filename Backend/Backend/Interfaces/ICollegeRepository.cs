using Backend.Models;

namespace Backend.Interfaces;

public interface ICollegeRepository
{
    ICollection<College> GetColleges();
    College GetCollege(int id);
    College GetCollegeByCity(int cityId);
    ICollection<Student> GetStudentsByCollege(int id);
}