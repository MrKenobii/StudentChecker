namespace Backend.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Company> Companies { get; set; }
    public ICollection<College> Colleges { get; set; }
    public ICollection<Student> Students { get; set; }
}