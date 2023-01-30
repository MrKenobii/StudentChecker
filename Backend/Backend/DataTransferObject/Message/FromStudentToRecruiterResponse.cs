namespace Backend.DataTransferObject.Message;

public class FromStudentToRecruiterResponse
{
    public string Content { get; set; }
    public int? FromStudentId { get; set; }
    public int? ToRecruiterId { get; set; }
}