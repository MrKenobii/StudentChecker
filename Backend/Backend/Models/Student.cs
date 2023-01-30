using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime EnrollDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Languages { get; set; }
    public string? Skills { get; set; }
    public string? Phone { get; set; }
    public string? Department { get; set; }
    public College? College { get; set; } // College
    public string? Token { get; set; }
    public byte[] Image { get; set; } 
    public City? City { get; set; } // City
    public bool IsActivated { get; set; }
    public string? VerifyToken { get; set; }
    public ICollection<SendMessage> SendMessages { get; set; }
    public ICollection<DeliveredMessage> DeliveredMessages { get; set; }
    
    public ICollection<StudentCourse>? StudentCourses { get; set; }
    
}