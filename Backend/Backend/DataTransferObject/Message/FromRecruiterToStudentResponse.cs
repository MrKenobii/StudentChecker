namespace Backend.DataTransferObject.Message;

public class FromRecruiterToStudentResponse
{
    public string Content { get; set; }
    public int? FromRecruiterId { get; set; }
    public int? ToStudentId { get; set; }
}