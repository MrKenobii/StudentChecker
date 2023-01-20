using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICollegeRepository
{
    ICollection<CollegeGetResponse> GetColleges();
    College GetCollege(int id);
    CollegeGetResponse GetCollegeById(int collegeId);
    CityDto GetCityByCollege(int college);
    ICollection<Student> GetStudentsByCollege(int id);
    bool CollegeExists(int collegeId);
    CollegePostResponse CreateCollege(CollegePostRequest collegeDto);
    CollegePostResponse UpdateCollege(int collegeId, CollegePostRequest collegeDto);
    void DeleteCollege(int collegeId);
}