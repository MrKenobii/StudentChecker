using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class DeliveredMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Student? Student { get; set; }
    public Recruiter? Recruiter { get; set; }
    public DateTime DeliveredTime { get; set; }
    public string Content { get; set; }
}