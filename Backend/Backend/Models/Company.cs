using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string CompanyType { get; set; }
    public string Email { get; set; }
    public DateTime FoundationDate { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public City City { get; set; }
    public String CompanyKey { get; set; }
    public ICollection<RecruiterCompany> RecruiterCompanies { get; set; }
}