namespace Backend.DataTransferObject.Admin;

public class AdminPostRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
}