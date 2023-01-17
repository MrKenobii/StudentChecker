using Backend.DataTransferObject.Recruiter;

namespace Backend.DataTransferObject;

public class AddRecruiterToCompany
{
    public IEnumerable<RecruiterRequestById> Recruiters { get; set; }
}