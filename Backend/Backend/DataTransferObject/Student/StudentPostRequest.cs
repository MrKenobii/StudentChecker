namespace Backend.DataTransferObject;

public class StudentPostRequest
{
    
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime EnrollDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Languages { get; set; }
    public string Skills { get; set; }
    public string Phone { get; set; }
    public string Department { get; set; }
    public string CollegeName { get; set; } // College
    public string Token { get; set; }
    public byte[] Image { get; set; } 
    public string CityName { get; set; } // City
    public bool IsActivated { get; set; }
    public string VerifyToken { get; set; }
}