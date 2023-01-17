namespace Backend.DataTransferObject.Recruiter;

public class RecruiterUpdateProfile
{
    public string Address { get; set; }
    public DateTime HireDate { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CompanyName { get; set; }
    public byte[] Image { get; set; }
}