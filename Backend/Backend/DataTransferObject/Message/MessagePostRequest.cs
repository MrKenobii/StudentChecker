namespace Backend.DataTransferObject.Message;

public class MessagePostRequest
{
    public int FromId { get; set; }
    public int ToId { get; set; }
    public string Content { get; set; }
}