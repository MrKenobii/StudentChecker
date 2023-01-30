namespace Backend.DataTransferObject.Message;

public class FromStudentToStudentResponse
{
    public string Content { get; set; }
    public int? FromStudentId { get; set; }
    public int? ToStudentId { get; set; }
}