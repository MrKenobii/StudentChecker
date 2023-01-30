namespace Backend.DataTransferObject.Message;

public class GetDeliveredMessageResponse
{
    public int? StudentId { get; set; }
    public int? RecruiterId { get; set; }
    public string Content { get; set; }
}