namespace StationeryAndSuppliesWebApp.Models; 

public class LoggedInUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;   

    public LoggedInUser() { }   

    public LoggedInUser(int id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
}
