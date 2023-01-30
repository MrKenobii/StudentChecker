using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class SendMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Student? Student { get; set; }
    public Recruiter? Recruiter { get; set; }
    public DateTime SendTime { get; set; }
    public string Content { get; set; }
}