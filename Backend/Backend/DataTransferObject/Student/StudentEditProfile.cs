namespace Backend.DataTransferObject;

public class StudentEditProfile
{
    public string Name { get; set; } //
    public string LastName { get; set; } //
    public string CollegeName { get; set; } //
    public string Email { get; set; } // 
    public string Department { get; set; }
    public string Address { get; set; } //
    public DateTime EnrollDate { get; set; } //
    public string Phone { get; set; } //
    public DateTime DateOfBirth { get; set; } //
    public string CityName { get; set; } //
    public string Skills { get; set; } //
    public string Languages { get; set; } //
    public List<CoursePostRequest> Courses { get; set; } //
    public byte[] Image { get; set; } // 
}