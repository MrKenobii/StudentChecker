using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICollegeRepository
{
    ICollection<College> GetColleges();
    College GetCollege(int id);
    College GetCollegeByCity(int cityId);
    ICollection<Student> GetStudentsByCollege(int id);
    bool CollegeExists(int collegeId);
    College CreateCollege(CollegeDto collegeDto);
    College UpdateCollege(int collegeId, CollegeDto collegeDto);
    void DeleteCollege(int collegeId);
}