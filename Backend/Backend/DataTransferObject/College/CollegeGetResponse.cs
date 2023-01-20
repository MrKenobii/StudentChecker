namespace Backend.DataTransferObject;

public class CollegeGetResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime FoundationDate { get; set; }
    public string EmailExtension { get; set; }
    public CityDto City { get; set; }
    public ICollection<StudentDto> Students { get; set; }
}