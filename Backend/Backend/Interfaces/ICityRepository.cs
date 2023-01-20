using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface ICityRepository
{
    ICollection<CityResponse> GetCities();
    City GetCity(int id);
    CityResponse GetCityResponse(int id);
    bool CityExists(int id);
    List<CompanyDto> GetCompaniesByCity(int id);
    List<CollegeDto> GetCollegesByCity(int id);
    List<StudentDto> GetStudentsByCity(int id);
    CityPostResponse CreateCity(CityPostRequest cityDto);
    CityPostResponse UpdateCity(int cityId, CityPostRequest cityDto);
    void DeleteCity(int cityId);
    City GetCityByName(string cityName);
}