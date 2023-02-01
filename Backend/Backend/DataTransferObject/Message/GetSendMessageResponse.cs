using Backend.Models;

namespace Backend.DataTransferObject.Message;

public class GetSendMessageResponse
{
    public int? StudentId { get; set; }
    public int? RecruiterId { get; set; }
    public string Content { get; set; }
    public DateTime? SendTime { get; set; }
    public DateTime? DeliveredTime { get; set; }
    public byte[]? RecruiterImage { get; set; }
    public string? RecruiterName { get; set; }
    public string? RecruiterLastName { get; set; }
    public byte[]? StudentImage { get; set; }
    public string? StudentName { get; set; }
    public string? StudentLastName { get; set; }
}