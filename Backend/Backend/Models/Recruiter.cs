using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Recruiter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Token { get; set; }
    public byte[] Image { get; set; }
    public bool IsActivated { get; set; }
    public string? VerifyToken { get; set; }
    public ICollection<SendMessage> SendMessages { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<DeliveredMessage> DeliveredMessages { get; set; }
    public ICollection<RecruiterCompany>? RecruiterCompanies { get; set; }
}