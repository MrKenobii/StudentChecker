using Backend.Models;

namespace Backend.DataTransferObject;

public class StudentDto
{
    public int Id { get; set; }
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
    public string Image { get; set; } 
    public string CityName { get; set; } // City
    public bool IsActivated { get; set; }
}