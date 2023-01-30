namespace Backend.DataTransferObject.Message;

public class GetSendMessageResponse
{
    public int? StudentId { get; set; }
    public int? RecruiterId { get; set; }
    public string Content { get; set; }
    public DateTime? SendTime { get; set; }
    public DateTime? DeliveredTime { get; set; }
}