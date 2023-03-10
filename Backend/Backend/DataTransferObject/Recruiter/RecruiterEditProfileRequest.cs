namespace Backend.DataTransferObject.Recruiter;

public class RecruiterEditProfileRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime HireDate { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CompanyName { get; set; }
    public byte[] Image { get; set; }
}