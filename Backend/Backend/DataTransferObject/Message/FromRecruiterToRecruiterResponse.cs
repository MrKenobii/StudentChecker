namespace Backend.DataTransferObject.Message;

public class FromRecruiterToRecruiterResponse
{
    public string Content { get; set; }
    public int? FromRecruiterId { get; set; }
    public int? ToRecruiterId { get; set; }
}