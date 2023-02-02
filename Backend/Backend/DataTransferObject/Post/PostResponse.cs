using Backend.Models;

namespace Backend.DataTransferObject.Post;

public class PostResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Models.Recruiter? Recruiter { get; set; }
    public Student? Student { get; set; }
    public DateTime CreatedTime { get; set; }
}