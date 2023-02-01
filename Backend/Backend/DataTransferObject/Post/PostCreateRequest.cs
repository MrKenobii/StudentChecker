namespace Backend.DataTransferObject.Post;

public class PostCreateRequest
{
    public string Content { get; set; }
    public string Title { get; set; }
    public int? RecruiterId { get; set; }
    public int? StudentId { get; set; }
}