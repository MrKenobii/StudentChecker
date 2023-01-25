namespace Backend.DataTransferObject.Recruiter;

public class ChangePasswordRequest
{
    public string PrevPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordCopy { get; set; }
}