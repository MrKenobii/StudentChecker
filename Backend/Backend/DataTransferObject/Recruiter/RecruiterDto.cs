namespace Backend.DataTransferObject;

public class RecruiterDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public byte[]? Image { get; set; }
    public bool IsActivated { get; set; }
}