namespace Backend.Models;

public class RecruiterCompany
{
    public int RecruiterId { get; set; }
    public int CompanyId { get; set; }
    public Recruiter Recruiter { get; set; }
    public Company Company { get; set; }
}