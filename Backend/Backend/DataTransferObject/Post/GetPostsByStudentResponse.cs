namespace Backend.DataTransferObject.Post;

public class GetPostsByStudentResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedTime { get; set; }
}