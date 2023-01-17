namespace Backend.DataTransferObject;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CompanyType { get; set; }
    public string Email { get; set; }
    public DateTime FoundationDate { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string CityName { get; set; }
    public string CompanyKey { get; set; }
}