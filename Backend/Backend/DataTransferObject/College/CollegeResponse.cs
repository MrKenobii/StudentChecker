namespace Backend.DataTransferObject;

public class CollegeResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime FoundationDate { get; set; }
    public string CityName { get; set; }
    public string EmailExtension { get; set; }
    public int StudentCount { get; set; }
}