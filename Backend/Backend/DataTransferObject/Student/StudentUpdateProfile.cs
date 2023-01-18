namespace Backend.DataTransferObject;

public class StudentUpdateProfile
{
    public string CollegeName { get; set; }
    public string Department { get; set; }
    public string Address { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CityName { get; set; }
    public string Skills { get; set; }
    public string Languages { get; set; }
    public List<CourseDto> Courses { get; set; }
    public byte[] Image { get; set; }
}