using Backend.Models;

namespace Backend.DataTransferObject;

public class CityResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<CollegeResponse> Colleges { get; set; }
    public ICollection<StudentDto> Students { get; set; }
    public ICollection<CompanyDto> Companies { get; set; }
}